using ExpenseTracker.Common;
using ExpenseTracker.Models;

namespace ExpenseTracker.Contracts
{
    public interface IDashboardRepository
    {

        Task <IEnumerable<Transaction>> GetLastTwoWeeksData();
        void DoughnutChartData();
        int TotalIncome();
        int TotalExpense();
        int Balance();

    }
}
