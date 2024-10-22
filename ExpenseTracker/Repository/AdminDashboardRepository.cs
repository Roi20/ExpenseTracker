using ExpenseTracker.Common;
using ExpenseTracker.Context;
using ExpenseTracker.Contracts;
using ExpenseTracker.Data;
using ExpenseTracker.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Syncfusion.EJ2.Linq;

namespace ExpenseTracker.Repository
{
    public class AdminDashboardRepository : IAdminDashboardRepository
    {
        private readonly DbContext _db;
        private readonly DbSet<Transaction> _transactions;
        private readonly UserManager<AppIdentityUser> _userManager;

        public AdminDashboardRepository(UserManager<AppIdentityUser> userManager, ExpenseTrackerDbContext db)
        {
            _db = db;
            _transactions =  _db.Set<Transaction>();
            _userManager = userManager;
        }

        public int ActiveUsersCount()
        {
            var activeUserCount =  _userManager.Users
                                               .Where(x => x.LastActivityDate >= DateTime.UtcNow.AddDays(-30))
                                               .Count();

            return activeUserCount;
        }

        public int InActiveUsersCount()
        {
            var inactiveUsersCount = _userManager.Users
                                                 .Where(x => x.LastActivityDate <= DateTime.UtcNow.AddDays(-30) || x.LastActivityDate == null)
                                                 .Count();

            return inactiveUsersCount;
        }

        public int RegisteredUsersCount()
        {

            var confirmedUserCount =   _userManager.Users.Count(c => c.EmailConfirmed);

            return confirmedUserCount;
        }

        public string[] Last12Months()
        {

            var months = Enumerable.Range(0, 12)
                                   .Select(s => DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(s)).ToString("MMM/yy"))
                                   .ToArray();


            /*
            var last12Months = Enumerable.Range(0, 12)
                                         .Select(i => DateTime.UtcNow.AddMonths(-i))
                                         .OrderBy(x => x.Year).ThenBy(d => d.Month)
                                         .Select(x => new GetMonthData{ Year = x.Year, Month = x.ToString("MMM/yy") })
                                         .ToArray();
            */
            return months;
            
        }

        public async Task<IEnumerable<FinancialTrendData>> GetFinancialTrendData()
        {

            try
            {
                var months = Last12Months();
                var averageIncome = await AverageIncomeSummary();
                var averageExpense = await AverageExpenseSummary();

                var data = months.Select(s => new FinancialTrendData
                {

                    Month = s,
                    AverageIncome = averageIncome.ContainsKey(s) ? averageIncome[s] : 0,
                    AverageExpense = averageExpense.ContainsKey(s) ? averageIncome[s] : 0

                    /*
                    Month = $"{s.Key.Month:00}/{s.Key.Year}",
                    AverageIncome = s.Where(x => x.Category.Type == "Income").Average(t => t.Amount),
                    AverageExpense = s.Where(x => x.Category.Type == "Expense").Average(t => t.Amount)
                    */
                }).ToList();


                return data;
            }
            catch
            {
                throw;
            }

        }

        public async Task<IEnumerable<Transaction>> GetTransactionData()
        {
            try
            {
                var data = await _transactions.Include(x => x.Category)
                                         .Where(t => t.Date >= DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(-12)) &&
                                         t.Date <= DateOnly.FromDateTime(DateTime.UtcNow))
                                         .ToListAsync();

                return data;
            }
            catch
            {
                throw;
            }
           
        }


        public async Task<Dictionary<string, double>> AverageExpenseSummary()
        {
            try
            {
                var data = await GetTransactionData();

                var expenseSummary = data.Where(x => x.Category.Type == "Expense")
                                         .GroupBy(g => g.Date)
                                         .ToDictionary(g =>

                                             g.Key.ToString("MMM/yy"),
                                             g => g.Average(a => a.Amount)

                                         );
                return expenseSummary;
            }
            catch
            {
                throw;
            }
           
        }

        public async Task<Dictionary<string, double>> AverageIncomeSummary()
        {
            try
            {
                var data = await GetTransactionData();

                var incomeSummary = data.Where(x => x.Category.Type == "Income")
                                         .GroupBy(g => g.Date)
                                         .ToDictionary(g =>

                                             g.Key.ToString("MMM/yy"),
                                             g => g.Average(a => a.Amount)

                                         );
                return incomeSummary;
            }
            catch(ArgumentException)
            {
                throw;
            }
        }



    }

}


