using ExpenseTracker.Common;
using ExpenseTracker.Models;

namespace ExpenseTracker.Contracts
{
    public interface IDashboardRepository
    {

        Task <IEnumerable<Transaction>> GetLastTwoWeeksData();
        Task <IEnumerable<ExpenseSummary>> DoughnutChartData();
        Task<int> TotalIncome();
        Task<int> TotalExpense();
        Task<int> Balance();

    }
}
