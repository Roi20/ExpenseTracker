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
                                   .Select(s => DateTime.UtcNow.AddMonths(-s))
                                   .OrderBy(d => d)
                                   .Select(x => x.ToString("MMM/yyyy"))
                                   .ToArray();


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
                    AverageExpense = averageExpense.ContainsKey(s) ? averageExpense[s] : 0

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
                                              .Where(t => t.Date >= DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(-12)) && t.Date <= DateOnly
                                              .FromDateTime(DateTime.UtcNow))
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
                                         .GroupBy(g => new { g.Date.Year, g.Date.Month, g.User_Id })
                                         .ToDictionary(g =>

                                             $"{new DateTime(g.Key.Year, g.Key.Month, 1):MMM/yyyy}/{g.Key.User_Id}",
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
                                         .GroupBy(g => new { g.Date.Year, g.Date.Month, g.User_Id })
                                         .ToDictionary(g =>

                                             $"{new DateTime(g.Key.Year, g.Key.Month, 1):MMM/yyyy}/{g.Key.User_Id}",
                                             g => g.Average(a => a.Amount)

                                         );
                return incomeSummary;
            }
            catch(ArgumentException)
            {
                throw;
            }
        }

        public async Task<IEnumerable<ModeData>> GetModeData()
        {
            try
            {
                var months = Last12Months();
                var modeIncome = await ModeIncomeSummary();
                var modeExpense = await ModeExpenseSummary();

                var data = months.Select(s => new ModeData
                {

                    Month = s,
                    ModeIncome = modeIncome.ContainsKey(s) ? modeIncome[s] : 0,
                    ModeExpense = modeExpense.ContainsKey(s) ? modeExpense[s] : 0

                }).ToList();


                return data;
            }
            catch
            {
                throw;
            }
        }
        public async Task<Dictionary<string, double>> ModeIncomeSummary()
        {
            try
            {
                var data = await GetTransactionData();

                var modeIncome = data.Where(x => x.Category.Type == "Income")
                                         .GroupBy(g => new { g.Date.Year, g.Date.Month, g.User_Id})
                                         .ToDictionary(g =>

                                             $"{new DateTime(g.Key.Year, g.Key.Month, 1):MMM/yyyy}/{g.Key.User_Id}",
                                             g => g.GroupBy(t => (double)t.Amount)
                                                   .OrderByDescending(x => x.Count())
                                                   .Select(x => x.Key)
                                                   .FirstOrDefault()

                                         );
                return modeIncome;
            }
            catch (ArgumentException)
            {
                throw;
            }
        }

        public async Task<Dictionary<string, double>> ModeExpenseSummary()
        {
            try
            {
                var data = await GetTransactionData();

                var modeIncome = data.Where(x => x.Category.Type == "Expense")
                                         .GroupBy(g => new { g.Date.Year, g.Date.Month, g.User_Id })
                                         .ToDictionary(g =>

                                             $"{new DateTime(g.Key.Year, g.Key.Month, 1):MMM/yyyy}/{g.Key.User_Id}",
                                             g => g.GroupBy(t => (double)t.Amount)
                                                   .OrderByDescending(x => x.Count())
                                                   .Select(x => x.Key)
                                                   .FirstOrDefault()

                                         );
                return modeIncome;
            }
            catch (ArgumentException)
            {
                throw;
            }
        }


    }

}


