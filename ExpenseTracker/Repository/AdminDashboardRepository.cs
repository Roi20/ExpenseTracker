using ExpenseTracker.Common;
using ExpenseTracker.Context;
using ExpenseTracker.Contracts;
using ExpenseTracker.Data;
using ExpenseTracker.Models;
using Humanizer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Syncfusion.EJ2.Linq;

namespace ExpenseTracker.Repository
{
    public class AdminDashboardRepository : IAdminDashboardRepository
    {
        private readonly DbContext _db;
        private readonly DbSet<Transaction> _transactions;
        private readonly DbSet<Category> _category;
        private readonly UserManager<AppIdentityUser> _userManager;

        public AdminDashboardRepository(UserManager<AppIdentityUser> userManager, ExpenseTrackerDbContext db)
        {
            _db = db;
            _transactions =  _db.Set<Transaction>();
            _category = _db.Set<Category>();
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

        public string[] Last6Months()
        {

            var months = Enumerable.Range(0, 6)
                                   .Select(s => DateTime.UtcNow.AddMonths(-s))
                                   .OrderBy(d => d)
                                   .Select(x => x.ToString("MMM/yyyy"))
                                   .ToArray();


            return months;
            
        }


        public async Task<IEnumerable<Transaction>> GetTransactionData()
        {
            try
            {
                var data = await _transactions.Include(x => x.Category)
                                              .Where(t => t.Date >= DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(-6)) && t.Date <= DateOnly
                                              .FromDateTime(DateTime.UtcNow))
                                              .ToListAsync();

                return data;
            }
            catch
            {
                throw;
            }
           
        }

        public async Task<IEnumerable<UserMonthlyAverage>> GetUserMonthlyAverages()
        {
            try
            {
                var data = await GetTransactionData();

                var userMonthlyAverages = data
                                             .GroupBy(g => new { g.User_Id, Month = g.Date.ToString("MMM/yyyy") })
                                             .Select(g => new UserMonthlyAverage
                                             {
                                                 UserId = g.Key.User_Id,
                                                 Month = g.Key.Month,
                                                 MonthlyAverageIncome = g.Where(x => x.Category.Type == "Income").Select(t => (double)t.Amount).DefaultIfEmpty(0).Average(),
                                                 MonthlyAverageExpense = g.Where(x => x.Category.Type == "Expense").Select(t => (double)t.Amount).DefaultIfEmpty(0).Average()
                                                 
                                             }).ToList();

                return userMonthlyAverages;
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
           
        }

        public async Task<IEnumerable<FinancialTrendData>> GetOveraAllMonthlyAverages()
        {

            try
            {
                
                var userMonthAverages = await GetUserMonthlyAverages();

                var months = Last6Months();

                var overAllMonthlyAverages = userMonthAverages
                                                              .GroupBy(u => u.Month)
                                                              .ToDictionary(
                                                                 g => g.Key,
                                                                 g => new FinancialTrendData
                                                                 {
                                                                     Month = g.Key,

                                                                     AverageIncome = g.Where(x => x.MonthlyAverageIncome > 0).Any() ?
                                                                                     g.Where(x => x.MonthlyAverageIncome > 0).Average(x => x.MonthlyAverageIncome) : 0,

                                                                     AverageExpense = g.Where(x => x.MonthlyAverageExpense > 0).Any() ?
                                                                                      g.Where(x => x.MonthlyAverageExpense > 0).Average(x => x.MonthlyAverageExpense) : 0
                                                                 }
                                                               );

                var result = months.Select(month => overAllMonthlyAverages.ContainsKey(month) ?
                                                    overAllMonthlyAverages[month] : new FinancialTrendData
                                                    {
                                                        Month = month,
                                                        AverageExpense = 0,
                                                        AverageIncome = 0

                                                    }).ToList();

                return result;
            }
            catch(Exception)
            {
                throw;
            }

        }

        public async Task<IEnumerable<UserMonthlyMode>> GetUserMonthlyMode()
        {
            try
            {
                var data = await GetTransactionData();

                var userMonthlyMode = data
                                          .GroupBy(g => new { g.User_Id, Month = g.Date.ToString("MMM/yyyy") })
                                          .Select(s => new UserMonthlyMode
                                          {
                                              UserId = s.Key.User_Id,
                                              Month = s.Key.Month,

                                              ModeExpense = s.Where(x => x.Category.Type == "Expense")
                                                             .GroupBy(g => g.Amount)
                                                             .OrderByDescending(x => x.Count())
                                                             .Select(x => x.Key)
                                                             .FirstOrDefault(),

                                              ModeIncome = s.Where(x => x.Category.Type == "Income")
                                                            .GroupBy(g => g.Amount)
                                                            .OrderByDescending(x => x.Count())
                                                            .Select(s => s.Key)
                                                            .FirstOrDefault()


                                          }).ToList();

                return userMonthlyMode;


            }
            catch (ArgumentException)
            {
                throw;
            }
            catch(Exception)
            {
                throw;
            }

        }

        public async Task<IEnumerable<ModeData>> GetOverallMonthlyMode()
        {
            try
            {
                var userMonthlyMode = await GetUserMonthlyMode();

                var months = Last6Months();

                var overallMonthlyMode = userMonthlyMode.GroupBy(g => g.Month)
                                                        .ToDictionary(g => g.Key, g => new ModeData
                                                        {
                                                            Month = g.Key,

                                                            ModeExpense = g.Where(x => x.ModeExpense > 0)
                                                                           .GroupBy(x => x.ModeExpense)
                                                                           .OrderByDescending(x => x.Count())
                                                                           .Select(s => s.Key)
                                                                           .FirstOrDefault(),

                                                            ModeIncome = g.Where(x => x.ModeIncome > 0)
                                                                          .GroupBy(x => x.ModeIncome)
                                                                          .OrderByDescending(x => x.Count())
                                                                          .Select(s => s.Key)
                                                                          .FirstOrDefault()

                                                        });



                var result = months.Select(month => overallMonthlyMode.ContainsKey(month) ? overallMonthlyMode[month]
                        : new ModeData
                        {
                            Month = month,
                            ModeExpense = 0,
                            ModeIncome = 0

                        }).ToList();


                return result;

            }
            catch(Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<TopListCategories>> TopCategories()
        {
            var data = await _category
                                      .GroupBy(g => g.Title)
                                      .OrderByDescending(x => x.Count())
                                      .Take(5)
                                      .Select(s => new TopListCategories
                                      {
                                          CategoryName = s.Key,
                                          CategoryCount = s.Count()


                                      }).ToListAsync();

            return data;

        }




        /*
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
        /*
        public async Task<IEnumerable<ModeData>> GetModeData()
        {
            try
            {
                var months = Last12Months();
                var modeIncome = await ModeIncomeSummary();
                var modeExpense = await ModeExpenseSummary();

                var data = months.Select(s => new ModeData
                {
                    /*
                    Month = s,
                    ModeIncome = modeIncome,
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
        */

    }

}


