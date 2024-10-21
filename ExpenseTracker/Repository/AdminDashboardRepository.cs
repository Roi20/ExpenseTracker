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

        public int ActiveUsersCount()
        {
            var activeUserCount =  _userManager.Users
                                               .Where(x => x.LastActivityDate >= DateTime.UtcNow.AddDays(-30))
                                               .Count();

            return activeUserCount;
        }

        public int InActiveUsersCount()
        {
            var inactiveUsersCount = _userManager.Users
                                                 .Where(x => x.LastActivityDate <= DateTime.UtcNow.AddDays(-30) || x.LastActivityDate == null)
                                                 .Count();

            return inactiveUsersCount;
        }

        public int RegisteredUsersCount()
        {

            var confirmedUserCount =   _userManager.Users.Count(c => c.EmailConfirmed);

            return confirmedUserCount;
        }
    }
}
