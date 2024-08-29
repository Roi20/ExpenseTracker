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

        public async Task<int> Balance()
        {

            var Income = await TotalIncome();
            var Expense = await TotalExpense();

            return Income - Expense;

        }


        public async Task<IEnumerable<Transaction>> GetLastTwoWeeksData()
        {

            var viewModel = new DashboardViewModel();

            var Data = await _transaction.Include(x => x.Category)
                                         .Where(t => t.Date >= viewModel.StartDate && t.Date <= viewModel.EndDate)
                                         .ToListAsync();

            return Data;
                                         
        
        }

        public async Task<int> TotalExpense()
        {
            var lastTwoWeeksData = await GetLastTwoWeeksData();

            var totalExpense = lastTwoWeeksData.Where(x => x.Category.Type == "Expense")
                                              .Sum(x => x.Amount);

            return totalExpense;
        }

        public async Task<int> TotalIncome()
        {
            var lastTwoWeeksData = await GetLastTwoWeeksData();

            var totalIncome = lastTwoWeeksData.Where(x => x.Category.Type == "Income")
                                              .Sum(x => x.Amount);

            return totalIncome;
        }


        public async Task<IEnumerable<ExpenseSummary>> DoughnutChartData()
        {
            var data = await GetLastTwoWeeksData();

            var dataSet =  data.Where(x => x.Category.Type == "Expense")
                               .GroupBy(i => i.Category.CategoryId)
                               .Select(x => new ExpenseSummary
                               {
                                   CategoryName = x.First().Category.Title,
                                   SumAmount = x.Sum(x => x.Amount),
                                   FormattedAmount = x.Sum(x => x.Amount).ToString("PHP #,##0")

                               })
                               .OrderByDescending(o => o.SumAmount)
                               .ToList();

            return dataSet;

        }


    }
}
