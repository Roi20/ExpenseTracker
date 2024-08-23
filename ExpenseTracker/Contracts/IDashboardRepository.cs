using ExpenseTracker.Common;
using ExpenseTracker.Models;

namespace ExpenseTracker.Contracts
{
    public interface IDashboardRepository
    {

        Task <IEnumerable<Transaction>> GetLastTwoWeeksData();
        int TotalIncome();
        int TotalExpense();
        int Balalance();

    }
}
