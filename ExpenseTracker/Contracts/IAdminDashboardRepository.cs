using ExpenseTracker.Common;

namespace ExpenseTracker.Contracts
{
    public interface IAdminDashboardRepository
    {

        int RegisteredUsersCount();
        int ActiveUsersCount();
        int InActiveUsersCount();
        Task <IEnumerable<FinancialTrendData>> GetOveraAllMonthlyAverages();
        Task<IEnumerable<ModeData>> GetOverallMonthlyMode();
        Task<IEnumerable<TopListCategories>> TopCategories();



    }
}
