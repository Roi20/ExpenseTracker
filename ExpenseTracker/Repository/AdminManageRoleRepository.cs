using ExpenseTracker.Common;
using ExpenseTracker.Contracts;
using ExpenseTracker.Data;
using Microsoft.AspNetCore.Identity;

namespace ExpenseTracker.Repository
{
    public class AdminManageRoleRepository : IAdminManageRoleRepository
    {
        private readonly UserManager<AppIdentityUser> _userManager;

        public AdminManageRoleRepository(UserManager<AppIdentityUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<IEnumerable<Moderators>> GetUserIsInRoleModerator()
        {
            try
            {
                var usersIsInRoleAsModerator = await _userManager.GetUsersInRoleAsync("Moderator");

                var moderators =  usersIsInRoleAsModerator.Select(x => new Moderators
                {
                    UserId = x.Id,
                    Name = $"{x.FirstName} {x.LastName}",
                    Email = x.Email,
                    Business = x.SourceOfIncome

                });

                return moderators;
            }
            catch (Exception) 
            {
                throw;
            }

        }
    }
}
