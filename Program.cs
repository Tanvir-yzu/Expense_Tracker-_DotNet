using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace ExpenseTracker
{
    public class Expense
    {
        public int Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Category { get; set; } = "General";
        public DateTime Date { get; set; }

        public override string ToString()
        {
            return $"[{Id}] {Date:yyyy-MM-dd} | {Category,-12} | {Description,-20} | {Amount:C2}";
        }
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
            // Set encoding for currency symbols
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            LoadData();

            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("==============================================");
                Console.WriteLine("           MODERN EXPENSE TRACKER             ");
                Console.WriteLine("==============================================");
                Console.ResetColor();

                DisplayBudgetStatus();

                Console.WriteLine("\n1. ➕ Add New Expense");
                Console.WriteLine("2. 🔍 List & Advanced Filter");
                Console.WriteLine("3. 📝 Update Expense");
                Console.WriteLine("4. ❌ Delete Expense");
                Console.WriteLine("5. 📊 Monthly Summary");
                Console.WriteLine("6. 💰 Set Monthly Budget");
                Console.WriteLine("7. 💾 Save & Exit");
                Console.Write("\nSelect an option (1-7): ");

                var choice = Console.ReadLine();
                switch (choice)
                {
                    case "1": AddExpense(); break;
                    case "2": FilterExpenses(); break;
                    case "3": UpdateExpense(); break;
                    case "4": DeleteExpense(); break;
                    case "5": GenerateSummary(); break;
                    case "6": SetBudget(); break;
                    case "7": SaveData(); return;
                    default:
                        Console.WriteLine("Invalid choice. Press any key...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        static void DisplayBudgetStatus()
        {
            if (data.MonthlyBudget <= 0) return;

            var currentMonthTotal = data.Expenses
                .Where(e => e.Date.Month == DateTime.Now.Month && e.Date.Year == DateTime.Now.Year)
                .Sum(e => e.Amount);

            decimal percent = (currentMonthTotal / data.MonthlyBudget) * 100;

            Console.Write($"Monthly Budget: {data.MonthlyBudget:C2} | Spent: ");
            
            if (currentMonthTotal > data.MonthlyBudget) Console.ForegroundColor = ConsoleColor.Red;
            else if (percent > 80) Console.ForegroundColor = ConsoleColor.Yellow;
            else Console.ForegroundColor = ConsoleColor.Green;

            Console.WriteLine($"{currentMonthTotal:C2} ({percent:F1}%)");
            Console.ResetColor();
        }

        static void AddExpense()
        {
            Console.WriteLine("\n--- Add New Expense ---");
            
            Console.Write("Description: ");
            string desc = Console.ReadLine() ?? "Unnamed";

            Console.Write("Category (e.g., Food, Transport, Utilities): ");
            string cat = Console.ReadLine() ?? "General";
            if (string.IsNullOrWhiteSpace(cat)) cat = "General";

            Console.Write("Amount: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal amt)) return;

            Console.Write("Date (yyyy-MM-dd) [Leave blank for today]: ");
            string dateIn = Console.ReadLine();
            DateTime dt = string.IsNullOrWhiteSpace(dateIn) 
                ? DateTime.Now 
                : (DateTime.TryParse(dateIn, out DateTime d) ? d : DateTime.Now);

            data.Expenses.Add(new Expense
            {
                Id = data.NextId++,
                Description = desc,
                Category = cat,
                Amount = amt,
                Date = dt
            });

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Expense recorded successfully!");
            Console.ResetColor();
            Console.ReadKey();
        }

        static void FilterExpenses()
        {
            Console.WriteLine("\n--- Filter Options ---");
            Console.WriteLine("1. Show All");
            Console.WriteLine("2. By Category");
            Console.WriteLine("3. By Date Range");
            Console.WriteLine("4. By Minimum Amount");
            Console.WriteLine("5. Search Description");
            Console.Write("Choice: ");
            var choice = Console.ReadLine();

            IEnumerable<Expense> query = data.Expenses;

            switch (choice)
            {
                case "2":
                    var cats = data.Expenses.Select(e => e.Category).Distinct().ToList();
                    Console.WriteLine($"Available: {string.Join(", ", cats)}");
                    Console.Write("Enter Category: ");
                    string filterCat = Console.ReadLine()?.ToLower() ?? "";
                    query = query.Where(e => e.Category.ToLower() == filterCat);
                    break;
                case "3":
                    Console.Write("Start Date (yyyy-MM-dd): ");
                    DateTime.TryParse(Console.ReadLine(), out DateTime start);
                    Console.Write("End Date (yyyy-MM-dd): ");
                    DateTime.TryParse(Console.ReadLine(), out DateTime end);
                    query = query.Where(e => e.Date >= start && e.Date <= end);
                    break;
                case "4":
                    Console.Write("Min Amount: ");
                    decimal.TryParse(Console.ReadLine(), out decimal min);
                    query = query.Where(e => e.Amount >= min);
                    break;
                case "5":
                    Console.Write("Search keyword: ");
                    string keyword = Console.ReadLine()?.ToLower() ?? "";
                    query = query.Where(e => e.Description.ToLower().Contains(keyword));
                    break;
            }

            DisplayResults(query.OrderByDescending(e => e.Date).ToList());
            Console.WriteLine("\nPress any key to return...");
            Console.ReadKey();
        }

        static void DisplayResults(List<Expense> results)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("{0,-5} | {1,-10} | {2,-12} | {3,-25} | {4,10}", "ID", "Date", "Category", "Description", "Amount");
            Console.WriteLine(new string('-', 75));
            Console.ResetColor();

            if (!results.Any())
            {
                Console.WriteLine("   No expenses found.");
                return;
            }

            foreach (var e in results)
            {
                if (e.Amount > 100) Console.ForegroundColor = ConsoleColor.Red;
                
                string desc = e.Description.Length > 25 ? e.Description.Substring(0, 22) + "..." : e.Description;
                string cat = e.Category.Length > 12 ? e.Category.Substring(0, 10) + ".." : e.Category;

                Console.WriteLine("{0,-5} | {1:yyyy-MM-dd} | {2,-12} | {3,-25} | {4,10:C2}", 
                    e.Id, e.Date, cat, desc, e.Amount);
                Console.ResetColor();
            }

            decimal total = results.Sum(r => r.Amount);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(new string('-', 75));
            Console.WriteLine("{0,63} | {1,10:C2}", "TOTAL", total);
            Console.ResetColor();
        }

        static void UpdateExpense()
        {
            Console.Write("\nEnter Expense ID to Update: ");
            if (!int.TryParse(Console.ReadLine(), out int id)) return;

            var exp = data.Expenses.FirstOrDefault(e => e.Id == id);
            if (exp == null) { Console.WriteLine("Not found."); return; }

            Console.WriteLine("Leaving a field blank will keep current value.");
            
            Console.Write($"Description [{exp.Description}]: ");
            string desc = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(desc)) exp.Description = desc;

            Console.Write($"Category [{exp.Category}]: ");
            string cat = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(cat)) exp.Category = cat;

            Console.Write($"Amount [{exp.Amount}]: ");
            if (decimal.TryParse(Console.ReadLine(), out decimal amt)) exp.Amount = amt;

            Console.WriteLine("Expense updated!");
            Console.ReadKey();
        }

        static void DeleteExpense()
        {
            Console.Write("\nEnter Expense ID to Delete: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                int removed = data.Expenses.RemoveAll(e => e.Id == id);
                Console.WriteLine(removed > 0 ? "Deleted." : "ID not found.");
            }
            Console.ReadKey();
        }

        static void GenerateSummary()
        {
            Console.WriteLine("\n--- Monthly Summaries ---");
            var summary = data.Expenses
                .GroupBy(e => new { e.Date.Year, e.Date.Month })
                .OrderByDescending(g => g.Key.Year).ThenByDescending(g => g.Key.Month);

            foreach (var group in summary)
            {
                string monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(group.Key.Month);
                Console.WriteLine($"{monthName} {group.Key.Year}: {group.Sum(x => x.Amount):C2}");
            }
            Console.ReadKey();
        }

        static void SetBudget()
        {
            Console.Write("\nEnter Monthly Spending Limit: ");
            if (decimal.TryParse(Console.ReadLine(), out decimal budget))
            {
                data.MonthlyBudget = budget;
                Console.WriteLine("Budget Saved.");
            }
            Console.ReadKey();
        }

        static void SaveData()
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(data, options);
                File.WriteAllText(DataFilePath, json);
                Console.WriteLine("Data saved successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Save error: {ex.Message}");
            }
        }

        static void LoadData()
        {
            if (!File.Exists(DataFilePath)) return;
            try
            {
                string json = File.ReadAllText(DataFilePath);
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                data = JsonSerializer.Deserialize<UserData>(json, options) ?? new UserData();
            }
            catch
            {
                Console.WriteLine("Warning: Could not load data. Starting fresh.");
                data = new UserData();
            }
        }
    }
}