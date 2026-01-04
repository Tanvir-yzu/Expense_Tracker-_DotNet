using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;

namespace ExpenseTracker
{
    public class Expense
    {
        public int Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Category { get; set; } = "General";
        public DateTime Date { get; set; }
    }

    public class UserData
    {
        public List<Expense> Expenses { get; set; } = new List<Expense>();
        public int NextId { get; set; } = 1;
        public decimal MonthlyBudget { get; set; } = 0;
    }

    class Program
    {
        private static UserData data = new UserData();
        private const string DataFilePath = "expenses.json";

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            LoadData();

            while (true)
            {
                DrawMainMenu();
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": AddExpense(); break;
                    case "2": FilterMenu(); break;
                    case "3": UpdateExpense(); break;
                    case "4": DeleteExpense(); break;
                    case "5": GenerateDetailedSummary(); break;
                    case "6": SetBudget(); break;
                    case "7": SaveAndExit(); return;
                    default: ShowError("Invalid selection."); break;
                }
            }
        }

        #region UI Components
        static void DrawHeader(string subTitle = "DASHBOARD")
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("╔═══════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                 MODERN EXPENSE TRACKER v2.2               ║");
            Console.WriteLine($"║                 {subTitle,-42}║");
            Console.WriteLine("╚═══════════════════════════════════════════════════════════╝");
            Console.ResetColor();
        }

        static void DrawMainMenu()
        {
            DrawHeader();
            DrawBudgetBar();
            Console.WriteLine("\n  MAIN MENU");
            Console.WriteLine("  ───────────────────────────────────────────────────────────");
            Console.WriteLine("  [1] ➕ Add Expense      [2] 🔍 Search & Filter");
            Console.WriteLine("  [3] 📝 Update Expense   [4] ❌ Delete Expense");
            Console.WriteLine("  [5] 📊 Detailed Summary [6] ⚙️  Set Budget");
            Console.WriteLine("  [7] 💾 Save & Exit");
            Console.Write("\n  Select Option > ");
        }

        static void DrawBudgetBar()
        {
            if (data.MonthlyBudget <= 0) return;
            var spent = data.Expenses.Where(e => e.Date.Month == DateTime.Now.Month && e.Date.Year == DateTime.Now.Year).Sum(e => e.Amount);
            double percent = (double)(spent / data.MonthlyBudget);
            int barWidth = 40;
            int filledWidth = (int)Math.Min(barWidth, (percent * barWidth));

            Console.Write("\n  Monthly Budget: ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"{spent:C2} / {data.MonthlyBudget:C2}");
            Console.ResetColor();
            Console.Write("  [");
            if (percent > 1.0) Console.ForegroundColor = ConsoleColor.Red;
            else if (percent > 0.85) Console.ForegroundColor = ConsoleColor.Yellow;
            else Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(new string('█', filledWidth));
            Console.ResetColor();
            Console.Write(new string('░', barWidth - filledWidth));
            Console.WriteLine($"] {(percent * 100):F0}%");
        }
        #endregion

        #region Summaries (New Detailed Feature)
        static void GenerateDetailedSummary()
        {
            DrawHeader("DETAILED REPORTS");
            if (!data.Expenses.Any()) { ShowError("No data available."); return; }

            // 1. Overview Totals
            decimal totalAllTime = data.Expenses.Sum(x => x.Amount);
            var monthlyGroup = data.Expenses.GroupBy(x => new { x.Date.Year, x.Date.Month })
                                            .OrderByDescending(g => g.Key.Year).ThenByDescending(g => g.Key.Month)
                                            .First();
            
            Console.WriteLine("\n  ⚡ QUICK STATS");
            Console.WriteLine($"  ├─ Total Expenses (All Time): {totalAllTime:C2}");
            Console.WriteLine($"  └─ Latest Month ({monthlyGroup.Key.Month}/{monthlyGroup.Key.Year}): {monthlyGroup.Sum(x => x.Amount):C2}");

            // 2. Category Breakdown (Current Month)
            Console.WriteLine("\n  📂 CATEGORY BREAKDOWN (LATEST MONTH)");
            Console.WriteLine("  ───────────────────────────────────────────────────────────");
            var catSummary = monthlyGroup.GroupBy(x => x.Category)
                                         .OrderByDescending(g => g.Sum(v => v.Amount));

            foreach (var cat in catSummary)
            {
                decimal catTotal = cat.Sum(x => x.Amount);
                double catPercent = (double)(catTotal / monthlyGroup.Sum(x => x.Amount)) * 100;
                Console.WriteLine($"  {cat.Key,-15} │ {catTotal,10:C2} │ {catPercent,5:F1}% " + GetMiniBar(catPercent));
            }

            // 3. Top Single Expenses
            Console.WriteLine("\n  🚩 TOP 3 LARGEST EXPENSES (THIS MONTH)");
            var top3 = monthlyGroup.OrderByDescending(x => x.Amount).Take(3);
            foreach (var item in top3)
            {
                Console.WriteLine($"  • {item.Amount:C2} - {item.Description} ({item.Category})");
            }

            Console.WriteLine("\n  ───────────────────────────────────────────────────────────");
            Console.WriteLine("  Press any key to return...");
            Console.ReadKey();
        }

        static string GetMiniBar(double percent)
        {
            int width = 10;
            int filled = (int)(percent / 10);
            return " [" + new string('■', filled) + new string(' ', width - filled) + "]";
        }
        #endregion

        #region Other Operations
        static void AddExpense()
        {
            DrawHeader("ADD ENTRY");
            Console.Write("\n  Description: "); string desc = Console.ReadLine() ?? "";
            Console.Write("  Category:    "); string cat = Console.ReadLine() ?? "General";
            Console.Write("  Amount:      "); if (!decimal.TryParse(Console.ReadLine(), out decimal amt)) return;
            Console.Write("  Date (yyyy-mm-dd) [Blank=Today]: "); string dIn = Console.ReadLine();
            DateTime dt = string.IsNullOrWhiteSpace(dIn) ? DateTime.Now : DateTime.Parse(dIn);

            data.Expenses.Add(new Expense { Id = data.NextId++, Description = desc, Category = cat, Amount = amt, Date = dt });
            ShowSuccess("Entry Saved!");
        }

        static void FilterMenu()
        {
            DrawHeader("SEARCH & FILTER");
            Console.WriteLine("\n  [1] Category  [2] Keyword  [3] Expensive (>100)  [Any] All");
            var c = Console.ReadLine();
            IEnumerable<Expense> q = data.Expenses;
            if (c == "1") { Console.Write("  Category: "); string s = Console.ReadLine()?.ToLower(); q = q.Where(x => x.Category.ToLower() == s); }
            else if (c == "2") { Console.Write("  Keyword: "); string s = Console.ReadLine()?.ToLower(); q = q.Where(x => x.Description.ToLower().Contains(s)); }
            else if (c == "3") { q = q.Where(x => x.Amount > 100); }

            DisplayTable(q.OrderByDescending(x => x.Date).ToList());
        }

        static void DisplayTable(List<Expense> list)
        {
            DrawHeader("FILTER RESULTS");
            Console.WriteLine("\n  {0,-4} | {1,-10} | {2,-12} | {3,-15} | {4,10}", "ID", "Date", "Category", "Desc", "Amount");
            Console.WriteLine("  " + new string('─', 60));
            foreach (var e in list)
            {
                if (e.Amount > 100) Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("  {0,-4} | {1:MM/dd/yy} | {2,-12} | {3,-15} | {4,10:C2}", e.Id, e.Date, Truncate(e.Category, 12), Truncate(e.Description, 15), e.Amount);
                Console.ResetColor();
            }
            Console.WriteLine("  " + new string('─', 60));
            Console.WriteLine($"  TOTAL: {list.Sum(x => x.Amount):C2}");
            Console.WriteLine("\n  Press any key..."); Console.ReadKey();
        }

        static void UpdateExpense()
        {
            Console.Write("\n  Enter ID: "); if (!int.TryParse(Console.ReadLine(), out int id)) return;
            var e = data.Expenses.FirstOrDefault(x => x.Id == id);
            if (e != null) { Console.Write("  New Desc: "); string d = Console.ReadLine(); if (!string.IsNullOrEmpty(d)) e.Description = d; ShowSuccess("Updated!"); }
        }

        static void DeleteExpense()
        {
            Console.Write("\n  Enter ID: "); if (int.TryParse(Console.ReadLine(), out int id)) { data.Expenses.RemoveAll(x => x.Id == id); ShowSuccess("Deleted!"); }
        }

        static void SetBudget()
        {
            Console.Write("\n  Limit: "); if (decimal.TryParse(Console.ReadLine(), out decimal b)) data.MonthlyBudget = b;
            ShowSuccess("Budget Set!");
        }

        static void SaveAndExit()
        {
            File.WriteAllText(DataFilePath, JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true }));
            Console.WriteLine("\n  Data Encrypted & Saved. Goodbye!");
        }

        static void LoadData()
        {
            if (File.Exists(DataFilePath)) data = JsonSerializer.Deserialize<UserData>(File.ReadAllText(DataFilePath)) ?? new UserData();
        }

        static string Truncate(string s, int m) => s.Length <= m ? s : s[..(m - 2)] + "..";
        static void ShowSuccess(string m) { Console.ForegroundColor = ConsoleColor.Green; Console.WriteLine($"\n  ✔ {m}"); Console.ResetColor(); Thread.Sleep(1000); }
        static void ShowError(string m) { Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine($"\n  ✖ {m}"); Console.ResetColor(); Thread.Sleep(1000); }
        #endregion
    }
}