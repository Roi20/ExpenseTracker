using ExpenseTracker.Common;
using ExpenseTracker.Context;
using ExpenseTracker.Contracts;
using ExpenseTracker.Data;
using ExpenseTracker.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Repository
{
    public class AdminManageRoleRepository : BaseRepository<AuditLog>, IAdminManageRoleRepository
    {
        private readonly UserManager<AppIdentityUser> _userManager;
        private readonly IBaseRepository<AuditLog> _baseRepo;

        public AdminManageRoleRepository(UserManager<AppIdentityUser> userManager, 
                                         ExpenseTrackerDbContext db,
                                         IBaseRepository<AuditLog> baseRepo,
                                         IHttpContextAccessor httpContext) : base(db, httpContext, userManager)
        {
            _userManager = userManager;
            _baseRepo = baseRepo;
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

        public async Task RemoveUserAsModerator(AppIdentityUser user)
        {
            try
            {
                var currentUser = await _baseRepo.GetCurrentUser();
                
                var log = new AuditLog
                {
                    User_Id = currentUser.Id,
                    UserName = currentUser.UserName ?? currentUser.Email,
                    Role = await _userManager.IsInRoleAsync(currentUser, "Admin") ? "Admin" : "Moderator",
                    Action = "Remove user role as moderator.",
                    TimeStamp = $"{DateTime.UtcNow:g}",
                    EntityId = user.Id,
                    EntityType = "User Role",
                    Details = $"Removed {user.UserName?? user.Email} role as Moderator"
                };

                if(await _userManager.IsInRoleAsync(user, "Moderator"))
                {
                    await _userManager.RemoveFromRoleAsync(user, "Moderator");
                }
                else
                {
                    throw new ArgumentException("User is not a Moderator.");
                }

                await _baseRepo.CreateAuditLog(log);

            }
            catch (DbUpdateException)
            {
                throw;
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
