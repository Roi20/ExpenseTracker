using ExpenseTracker.Common;
using ExpenseTracker.Models;
using ExpenseTracker.ViewModel;

namespace ExpenseTracker.Contracts
{
    public interface IAdminDashboardRepository : IBaseRepository<AuditLog>
    {

        int RegisteredUsersCount();
        int ActiveUsersCount();
        int InActiveUsersCount();
        Task <IEnumerable<FinancialTrendData>> GetOveraAllMonthlyAverages();
        Task<IEnumerable<ModeData>> GetOverallMonthlyMode();
        Task<IEnumerable<TopListCategories>> TopCategories();
        Task<IEnumerable<Transaction>> RecentBiggestTransactions();
        Task<IEnumerable<Transaction>> RecentLowestTransactions();



    }
}
