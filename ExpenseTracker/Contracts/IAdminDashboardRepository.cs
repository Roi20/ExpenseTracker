namespace ExpenseTracker.Contracts
{
    public interface IAdminDashboardRepository
    {

        int RegisteredUsersCount();
        int ActiveUsersCount();
        int InActiveUsersCount();


    }
}
