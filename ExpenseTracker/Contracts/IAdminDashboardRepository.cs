using ExpenseTracker.Common;

namespace ExpenseTracker.Contracts
{
    public interface IAdminDashboardRepository
    {

        int RegisteredUsersCount();
        int ActiveUsersCount();
        int InActiveUsersCount();
       // IEnumerable<GetMonthData> Last12Months();
        Task <IEnumerable<FinancialTrendData>> GetFinancialTrendData();



    }
}
