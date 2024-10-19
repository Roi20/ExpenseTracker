using ExpenseTracker.Contracts;
using ExpenseTracker.Data;
using Microsoft.AspNetCore.Identity;

namespace ExpenseTracker.Repository
{
    public class AdminDashboardRepository : IAdminDashboardRepository
    {
        private readonly UserManager<AppIdentityUser> _userManager;

        public AdminDashboardRepository(UserManager<AppIdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public int RegisteredUsersCount()
        {

            var confirmedUserCount =   _userManager.Users.Count(c => c.EmailConfirmed);

            return confirmedUserCount;
        }
    }
}
