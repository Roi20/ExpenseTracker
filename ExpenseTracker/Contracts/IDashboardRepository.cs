using ExpenseTracker.Common;
using ExpenseTracker.Models;

namespace ExpenseTracker.Contracts
{
    public interface IDashboardRepository
    {

        Task <IEnumerable<Transaction>> GetData(DateOnly StartDate, DateOnly EndDate);
        Task <IEnumerable<ExpenseSummary>> DoughnutChartData(DateOnly startDate, DateOnly endDate);
        Task<int> TotalIncome(DateOnly startDate, DateOnly endDate);
        Task<int> TotalExpense(DateOnly startDate, DateOnly endDate);
        Task<int> Balance(DateOnly startDate, DateOnly endDate);
        string[] LastTwoWeeks(DateOnly startDate);
        Task<List<LineChartData>> IncomeSummary(DateOnly startDate, DateOnly endDate);


    }
}
