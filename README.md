# 💰 Modern Expense Tracker v2.3

<div align="center">

A powerful, intuitive console-based expense tracking application built with **.NET 9.0**

[![.NET](https://img.shields.io/badge/.NET-9.0-purple.svg)](https://dotnet.microsoft.com/download/dotnet/9.0)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)

**Track • Analyze • Budget**

</div>

---

## 📋 Table of Contents

- [Overview](#-overview)
- [Features](#-features)
- [Screenshots](#-screenshots)
- [Installation](#-installation)
- [Usage Guide](#-usage-guide)
- [Project Structure](#-project-structure)
- [Data Management](#-data-management)
- [Contributing](#-contributing)
- [License](#-license)

---

## 🌟 Overview

Modern Expense Tracker is a command-line application designed to help individuals and small businesses manage their expenses efficiently. With a clean, modern interface and powerful features, it makes expense tracking simple and intuitive.

### Why Choose This Tracker?

- ✨ **Modern UI**: Beautiful console interface with color-coded elements and visual progress bars
- 📊 **Smart Analytics**: Detailed reports and category breakdowns
- 💾 **Persistent Storage**: Automatic data saving in JSON format
- 🎯 **Budget Tracking**: Set and monitor monthly budgets with visual indicators
- 🔍 **Powerful Filtering**: Search by category, keywords, or amount thresholds
- 🚀 **Zero Dependencies**: No external database or cloud services required

---

## ✨ Features

### Core Functionality

| Feature | Description |
|---------|-------------|
| ➕ **Add Expenses** | Quick entry with description, category, amount, and date |
| 📝 **Edit Expenses** | Update any expense field with intuitive prompts |
| ❌ **Delete Expenses** | Remove expenses by ID with confirmation |
| 🔍 **Advanced Search** | Filter by category, keyword, or high-value items |
| 📊 **Detailed Reports** | Category breakdowns, monthly totals, and statistics |
| 💰 **Budget Management** | Set monthly limits with visual progress tracking |

### Visual Features

- 🎨 **Color-Coded Interface** - Expenses over $100 highlighted in red
- 📈 **Budget Progress Bar** - Visual representation of monthly spending
- ✅ **Success/Error Messages** - Clear feedback for all actions
- 📅 **Date Formatting** - Consistent, readable date displays

### Data Management

- 💾 **Auto-Save** - Data persists between sessions
- 🔄 **JSON Format** - Human-readable, easy to backup
- 🔢 **Unique IDs** - Automatic ID generation for each expense
- 📂 **Categorization** - Flexible category system

---

## 🖼️ Screenshots

### Main Dashboard
```
╔═══════════════════════════════════════════════════════════╗
║                 MODERN EXPENSE TRACKER v2.3               ║
║                 DASHBOARD                                  ║
╚═══════════════════════════════════════════════════════════╝

  Monthly Budget: $9,700.00 / $10,000.00
  [████████████████████████████░░░░░] 97%

  MAIN MENU
  ───────────────────────────────────────────────────────────
  [1] ➕ Add Expense      [2] 🔍 Search & Filter
  [3] 📝 Update Expense   [4] ❌ Delete Expense
  [5] 📊 Detailed Summary [6] ⚙️  Set Budget
  [7] 💾 Save & Exit

  Select Option >
```

### Detailed Reports
```
╔═══════════════════════════════════════════════════════════╗
║                 MODERN EXPENSE TRACKER v2.3               ║
║                 DETAILED REPORTS                           ║
╚═══════════════════════════════════════════════════════════╝

  ⚡ QUICK STATS
  ├─ Total Expenses (All Time): $10,200.00
  └─ Current Month Total:        $9,700.00

  📂 CATEGORY BREAKDOWN (THIS MONTH)
  ───────────────────────────────────────────────────────────
  Mobile          │    $7,500.00 │ 77.3% [██████████]
  school          │    $1,200.00 │ 12.4% [█         ]
  pen             │      $450.00 │  4.6% [          ]
  backpot         │      $300.00 │  3.1% [          ]
```

---

## 🚀 Installation

### Prerequisites

- **.NET 9.0 SDK** or later
  - Download from: https://dotnet.microsoft.com/download/dotnet/9.0
  - Verify installation: `dotnet --version`

### Setup Steps

1. **Clone or Download** the repository
   ```bash
   git clone <repository-url>
   cd Expense_Tracker
   ```

2. **Restore Dependencies** (if any)
   ```bash
   dotnet restore
   ```

3. **Build the Project**
   ```bash
   dotnet build
   ```

4. **Run the Application**
   ```bash
   dotnet run
   ```

### Build Configurations

| Command | Description |
|---------|-------------|
| `dotnet run` | Run in Debug mode with hot reload |
| `dotnet build` | Compile without running |
| `dotnet build -c Release` | Optimize for production |

---

## 📖 Usage Guide

### Getting Started

1. **Launch the Application**
   ```bash
   dotnet run
   ```

2. **Set Your Monthly Budget**
   - Select option `[6]` from the main menu
   - Enter your desired monthly spending limit

3. **Add Your First Expense**
   - Select option `[1]` from the main menu
   - Provide: Description, Category, Amount, Date

4. **Track and Analyze**
   - Use `[5]` to view detailed reports
   - Use `[2]` to search and filter expenses

### Quick Reference

#### Adding an Expense
```
Select Option > 1

  Description: Groceries
  Category: Food
  Amount: 150.50
  Date (yyyy-mm-dd) [Blank=Today]:

  ✔ Entry Saved!
```

#### Setting a Budget
```
Select Option > 6

  New Monthly Limit: 5000

  ✔ Budget Updated!
```

#### Searching Expenses
```
Select Option > 2

  [1] Category  [2] Keyword  [3] Expensive (>100)  [Any] All
  1
  Category: Food
```

#### Updating an Expense
```
Select Option > 3

  Enter Expense ID to modify: 1

  Editing ID: 1
  (Press ENTER to keep current value)
  ───────────────────────────────────
  Description [Groceries]: Weekly Groceries
  Category    [Food]: 
  Amount      [$150.50]: 180.75
  Date (yyyy-MM-dd) [01/04/26]:

  ✔ Expense updated successfully!
```

---

## 📁 Project Structure

```
Expense_Tracker/
├── Program.cs              # Main application logic
├── Expense_Tracker.csproj  # Project configuration
├── expenses.json           # Persistent data storage (auto-generated)
├── README.md               # This file
├── bin/                    # Compiled binaries
│   ├── Debug/
│   └── Release/
└── obj/                    # Build artifacts
    ├── Debug/
    └── Release/
```

### Key Components

#### `Program.cs` Architecture

```csharp
┌─────────────────────────────────────────┐
│           Main Program Flow             │
├─────────────────────────────────────────┤
│  ┌─────────────┐    ┌──────────────┐    │
│  │  Load Data  │───▶│  Main Loop   │    │
│  └─────────────┘    └──────────────┘    │
│                            │             │
│         ┌──────────────────┼────────────┐│
│         ▼                  ▼            ▼│
│  ┌─────────────┐  ┌─────────────┐ ┌──────────┐│
│  │   Add/      │  │   Search/   │ │ Reports  ││
│  │   Update    │  │   Filter    │ │ & Budget ││
│  │   Delete    │  │             │ │          ││
│  └─────────────┘  └─────────────┘ └──────────┘│
│                            │             │    │
│                            └──────┬──────┘    │
│                                   ▼          │
│                           ┌───────────────┐   │
│                           │  Save & Exit  │   │
│                           └───────────────┘   │
└─────────────────────────────────────────┘
```

#### Data Models

```csharp
public class Expense
{
    public int Id { get; set; }              // Unique identifier
    public string Description { get; set; }  // Expense description
    public decimal Amount { get; set; }      // Cost amount
    public string Category { get; set; }    // Expense category
    public DateTime Date { get; set; }      // Transaction date
}

public class UserData
{
    public List<Expense> Expenses { get; set; }  // All expenses
    public int NextId { get; set; }             // Auto-increment ID
    public decimal MonthlyBudget { get; set; }  // Budget limit
}
```

---

## 💾 Data Management

### File Format

Expenses are stored in `expenses.json` using the following structure:

```json
{
  "Expenses": [
    {
      "Id": 1,
      "Description": "Groceries",
      "Amount": 150.50,
      "Category": "Food",
      "Date": "2026-01-04T10:30:00"
    }
  ],
  "NextId": 2,
  "MonthlyBudget": 5000.00
}
```

### Backup Recommendations

1. **Regular Backups**: Copy `expenses.json` to a safe location
2. **Cloud Storage**: Store backup in Google Drive, Dropbox, etc.
3. **Version Control**: Track changes with Git (exclude sensitive data)
4. **Export Format**: JSON is portable and can be imported into other tools

### Data Recovery

If `expenses.json` is lost or corrupted:
1. Check your backup locations
2. Restore from the last known good version
3. Contact support if no backup is available

---

## 🔧 Customization

### Modifying Categories

Edit your expenses directly in `expenses.json` or use the Update feature:

```json
"Category": "New Category Name"
```

### Adding New Features

The codebase is modular and easy to extend:

1. **New Filters**: Add options to `FilterMenu()`
2. **Export Features**: Implement CSV/PDF export
3. **Date Ranges**: Add week/year filtering options
4. **Charts**: Integrate a charting library for visualizations

### Color Customization

Modify console colors in the `DrawHeader()` and `DrawBudgetBar()` methods:

```csharp
Console.ForegroundColor = ConsoleColor.Cyan;  // Header color
Console.ForegroundColor = ConsoleColor.Green; // Success color
Console.ForegroundColor = ConsoleColor.Red;   // Error color
```

---

## 🤝 Contributing

Contributions are welcome! Here's how you can help:

1. **Report Bugs**: Open an issue with detailed description
2. **Suggest Features**: Propose new functionality or improvements
3. **Submit PRs**: Fork, modify, and create a pull request
4. **Improve Docs**: Enhance documentation or add examples

### Development Guidelines

- Follow existing code style and conventions
- Add comments for complex logic
- Test thoroughly before submitting
- Update documentation for new features

---

## ❓ FAQ

**Q: Can I use this on Linux/Mac?**
A: Yes! .NET 9.0 is cross-platform compatible.

**Q: Is my data secure?**
A: Data is stored locally in JSON format. For enhanced security, encrypt the file or use encrypted storage.

**Q: Can I import data from other apps?**
A: Currently, only JSON format is supported. Import features can be added upon request.

**Q: What happens if I exceed my budget?**
A: The progress bar turns red, but you can still add expenses. This is a tracking tool, not a blocker.

**Q: How do I reset all data?**
A: Delete `expenses.json` and restart the application.

---

## 📄 License

This project is licensed under the MIT License - see the LICENSE file for details.

---

## 🙏 Acknowledgments

- Built with [.NET](https://dotnet.microsoft.com/)
- Inspired by modern financial management tools
- Designed for simplicity and efficiency

---

<div align="center">

**Made with ❤️ for better financial management**

[⬆ Back to Top](#-modern-expense-tracker-v23)

</div>
