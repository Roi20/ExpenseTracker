using ExpenseTracker.Common;
using ExpenseTracker.Context;
using ExpenseTracker.Contracts;
using ExpenseTracker.Data;
using ExpenseTracker.Models;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace ExpenseTracker.Repository
{
    public class DashboardRepository : IDashboardRepository
    {

        private readonly DbContext _db;
        private readonly DbSet<Transaction> _transaction;
        private readonly DbSet<AppIdentityUser> _user;
        private readonly DbSet<Notification> _notification;



        public DashboardRepository(ExpenseTrackerDbContext db)
        {
            _db = db;
            _transaction = _db.Set<Transaction>();
            _user = _db.Set<AppIdentityUser>();
            _notification = _db.Set<Notification>();
        }

        public async Task<int> Balance(DateOnly startDate, DateOnly endDate, string userId)
        {

            var Income = await TotalIncome(startDate, endDate, userId);
            var Expense = await TotalExpense(startDate, endDate, userId);


            var result = Income - Expense;

            return result <= 0 ? 0 : result;

        }


        public async Task<IEnumerable<Transaction>> GetData(DateOnly StartDate, DateOnly EndDate, string userId)
        {


            var Data = await _transaction.Include(x => x.Category)
                                         .Where(t => t.Date >= StartDate && 
                                                     t.Date <= EndDate &&
                                                     t.Category.User_Id == userId)
                                                      .ToListAsync();

            return Data;
                                         
        
        }

        public async Task<int> TotalExpense(DateOnly startDate, DateOnly endDate, string userId)
        {
   

            var Data = await GetData(startDate, endDate, userId);

            var totalExpense = Data.Where(x => x.Category.Type == "Expense")
                                   .Sum(x => x.Amount);

            return totalExpense;
        }

        public async Task<int> TotalIncome(DateOnly startDate, DateOnly endDate, string userId)
        {

            var Data = await GetData(startDate, endDate, userId);

            var totalIncome = Data.Where(x => x.Category.Type == "Income")
                                  .Sum(x => x.Amount);

            return totalIncome;
        }


        public async Task<IEnumerable<ExpenseSummary>> DoughnutChartData(DateOnly startDate, DateOnly endDate, string userId)
        {

            var data = await GetData(startDate, endDate, userId);

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

        public string[] DayRange(DateOnly startDate, int range)
        {

            var days = Enumerable.Range(0, range)
                                 .Select(x => startDate.AddDays(x).ToString("dd-MMM"))
                                 .ToArray();

            return days;

        }

        public async Task<Dictionary<string, int>> IncomeSummary(DateOnly startDate, DateOnly endDate, string userId)
        {
            var data = await GetData(startDate, endDate, userId);

            var incomeSummary = data.Where(x => x.Category.Type == "Income")
                                   .GroupBy(g => g.Date)
                                   .ToDictionary( g => 
                                        
                                      g.Key.ToString("dd-MMM"),
                                      g => g.Sum(a => a.Amount)
                                        
                                    );
                               

            return incomeSummary;

        }

        public async Task<Dictionary<string, int>> ExpenseSummary(DateOnly startDate, DateOnly endDate, string userId)
        {
            var data = await GetData(startDate, endDate, userId);

            var expenseSummary = data.Where(x => x.Category.Type == "Expense")
                                     .GroupBy(g => g.Date)
                                     .ToDictionary(g =>

                                         g.Key.ToString("dd-MMM"),
                                         g => g.Sum(a => a.Amount)
    
                                     );

            return expenseSummary;
        }

        public async Task<List<LineChartData>> GetLineChartData(DateOnly startDate, DateOnly endDate, int Range, string userId)
        {

            var days = DayRange(startDate, Range);
            var incomeData = await IncomeSummary(startDate, endDate, userId);
            var expenseData = await ExpenseSummary(startDate, endDate, userId);

            var LineChartData = days.Select(day => new LineChartData
            {
                NumberOfDays = day,
                Income = incomeData.ContainsKey(day) ? incomeData[day] : 0,
                Expense = expenseData.ContainsKey(day) ? expenseData[day] : 0,

            }).ToList();

            return LineChartData;
        }

        public async Task<IEnumerable<Transaction>> GetAllTransaction(string userId)
        {
            return await _transaction
                                     .Include(c => c.Category)
                                     .Where(x => x.User_Id == userId)
                                     .ToListAsync();
        }

        public async Task<AppIdentityUser> GetUserInfo(string userId)
        {
            return await _user.FirstOrDefaultAsync(x => x.Id == userId);
        }

        public async Task<IEnumerable<Notification>> GetAllUserNotification(string userId)
        {
            return await _notification.Where(x => x.UserId == userId).ToListAsync();
        }

        public async Task<Notification> MarkAsReadUserNotification(int id)
        {
            try
            {
                var notificationId = await _notification.FindAsync(id);

                if (notificationId == null)
                    throw new ArgumentException("Notification not found.");


                notificationId.IsRead = true;
                await _db.SaveChangesAsync();

                return notificationId;


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
    }
}
