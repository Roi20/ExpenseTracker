using ExpenseTracker.Common;
using ExpenseTracker.Models;

namespace ExpenseTracker.Contracts
{
    public interface IDashboardRepository
    {

        Task <IEnumerable<Transaction>> GetData(DateOnly StartDate, DateOnly EndDate, string userId);
        Task <IEnumerable<ExpenseSummary>> DoughnutChartData(DateOnly startDate, DateOnly endDate, string userId);
        Task<int> TotalIncome(DateOnly startDate, DateOnly endDate, string userId);
        Task<int> TotalExpense(DateOnly startDate, DateOnly endDate, string userId);
        Task<int> Balance(DateOnly startDate, DateOnly endDate, string userId);
        string[] DayRange(DateOnly startDate, int range);
        Task<Dictionary<string, int>> IncomeSummary(DateOnly startDate, DateOnly endDate, string userId);
        Task<Dictionary<string, int>> ExpenseSummary(DateOnly startDate, DateOnly endDate, string userId);
        Task<List<LineChartData>> GetLineChartData(DateOnly startDate, DateOnly endDate, int Range, string userId);
        Task<IEnumerable<Transaction>> GetAllTransaction(string userId);
    }
}
