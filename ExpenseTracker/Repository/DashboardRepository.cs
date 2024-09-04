using ExpenseTracker.Common;
using ExpenseTracker.Context;
using ExpenseTracker.Contracts;
using ExpenseTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Repository
{
    public class DashboardRepository : IDashboardRepository
    {

        private readonly DbContext _db;
        private readonly DbSet<Transaction> _transaction;


        public DashboardRepository(ExpenseTrackerDbContext db)
        {
            _db = db;
            _transaction = _db.Set<Transaction>();
        }

        public async Task<int> Balance(DateOnly startDate, DateOnly endDate)
        {

            var Income = await TotalIncome(startDate, endDate);
            var Expense = await TotalExpense(startDate, endDate);

            return Income - Expense;

        }


        public async Task<IEnumerable<Transaction>> GetData(DateOnly StartDate, DateOnly EndDate)
        {


            var Data = await _transaction.Include(x => x.Category)
                                         .Where(t => t.Date >= StartDate && t.Date <= EndDate)
                                         .ToListAsync();

            return Data;
                                         
        
        }

        public async Task<int> TotalExpense(DateOnly startDate, DateOnly endDate)
        {
   

            var lastTwoWeeksData = await GetData(startDate, endDate);

            var totalExpense = lastTwoWeeksData.Where(x => x.Category.Type == "Expense")
                                              .Sum(x => x.Amount);

            return totalExpense;
        }

        public async Task<int> TotalIncome(DateOnly startDate, DateOnly endDate)
        {

            var lastTwoWeeksData = await GetData(startDate, endDate);

            var totalIncome = lastTwoWeeksData.Where(x => x.Category.Type == "Income")
                                              .Sum(x => x.Amount);

            return totalIncome;
        }


        public async Task<IEnumerable<ExpenseSummary>> DoughnutChartData(DateOnly startDate, DateOnly endDate)
        {

            var data = await GetData(startDate, endDate);

            var dataSet =  data.Where(x => x.Category.Type == "Expense")
                               .GroupBy(i => i.Category.CategoryId)
                               .Select(e => new ExpenseSummary
                               {
                                   CategoryName = e.First().Category.Title,
                                   SumAmount = e.Sum(s => s.Amount),
                                   FormattedAmount = e.Sum(s => s.Amount).ToString("PHP #,##0")
                                   
                               })
                               .OrderByDescending(o => o.SumAmount)
                               .ToList();

            return dataSet;

        }

        public string[] LastTwoWeeks(DateOnly startDate, int range)
        {

            var days = Enumerable.Range(0, range)
                                 .Select(x => startDate.AddDays(x).ToString("dd-MMM"))
                                 .ToArray();

            return days;

        }

        public async Task<Dictionary<string, int>> IncomeSummary(DateOnly startDate, DateOnly endDate)
        {
            var data = await GetData(startDate, endDate);

            var incomeSummary = data.Where(x => x.Category.Type == "Income")
                                   .GroupBy(g => g.Date)
                                   .ToDictionary( g => 
                                        
                                      g.Key.ToString("dd-MMM"),
                                      g => g.Sum(a => a.Amount)
                                        
                                    );
                               

            return incomeSummary;

        }

        public async Task<Dictionary<string, int>> ExpenseSummary(DateOnly startDate, DateOnly endDate)
        {
            var data = await GetData(startDate, endDate);

            var expenseSummary = data.Where(x => x.Category.Type == "Expense")
                                     .GroupBy(g => g.Date)
                                     .ToDictionary(g =>

                                         g.Key.ToString("dd-MMM"),
                                         g => g.Sum(a => a.Amount)
    
                                     );

            return expenseSummary;
        }

        public async Task<List<LineChartData>> GetLineChartData(DateOnly startDate, DateOnly endDate, int Range)
        {

            var days = LastTwoWeeks(startDate, Range);
            var incomeData = await IncomeSummary(startDate, endDate);
            var expenseData = await ExpenseSummary(startDate, endDate);

            var LineChartData = days.Select(day => new LineChartData
            {
                NumberOfDays = day,
                Income = incomeData.ContainsKey(day) ? incomeData[day] : default,
                Expense = expenseData.ContainsKey(day) ? expenseData[day] : default,

            }).ToList();

            return LineChartData;
        }
    }
}
