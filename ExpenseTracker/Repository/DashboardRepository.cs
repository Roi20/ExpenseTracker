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

        public int Balalance()
        {

           return TotalIncome() - TotalExpense();

        }

        public async Task<IEnumerable<Transaction>> GetLastTwoWeeksData()
        {

            var viewModel = new DashboardViewModel();

            var Data = await _transaction.Include(x => x.Category)
                                         .Where(t => t.Date >= viewModel.StartDate && t.Date <= viewModel.EndDate)
                                         .ToListAsync();

            return Data;
                                         
        
        }

        public int TotalExpense()
        {
            var lastTwoWeeksData = GetLastTwoWeeksData().Result;

            var totalExpense = lastTwoWeeksData.Where(x => x.Category.Type == "Expense")
                                              .Sum(x => x.Amount);

            return totalExpense;
        }

        public int TotalIncome()
        {
            var lastTwoWeeksData = GetLastTwoWeeksData().Result;

            var totalIncome = lastTwoWeeksData.Where(x => x.Category.Type == "Income")
                                              .Sum(x => x.Amount);

            return totalIncome;
        }
    }
}
