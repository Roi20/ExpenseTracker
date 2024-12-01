using ExpenseTracker.Contracts;
using ExpenseTracker.Data;
using ExpenseTracker.Models;
using ExpenseTracker.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Controllers
{
    public class AdminManageRoleController : AdminBaseController
    {

        private readonly IAdminManageRoleRepository _repo;
        private readonly UserManager<AppIdentityUser> _userManager;
        private readonly IBaseRepository<AuditLog> _baseRepo;

        public AdminManageRoleController(IAdminManageRoleRepository repo, UserManager<AppIdentityUser> userManager, IBaseRepository<AuditLog> baseRepo)
        {
            _repo = repo;
            _userManager = userManager;
            _baseRepo = baseRepo;
        }

        public async Task<IActionResult> Index(AdminViewModel model)
        {
            model.Moderators = await _repo.GetUserIsInRoleModerator();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> RemoveUserAsModerator(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);

                if (user == null)
                    return NotFound("User Not Found.");


                await _repo.RemoveUserAsModerator(user);

                //Create audit log
                var currentUser = await _baseRepo.GetCurrentUser();

                await _baseRepo.CreateAuditLog(currentUser.Id,
                                               currentUser.UserName ?? currentUser.Email,
                                               await _userManager.IsInRoleAsync(currentUser, "Admin") ? "Admin" : "Moderator",
                                               "Remove user role as moderator.",
                                               DateTime.UtcNow.AddHours(8),
                                               user.Id,
                                               "User Role",
                                               $"Removed {user.UserName ?? user.Email} as Moderator");


                return Ok("User removed as moderator");

            }
            catch(DbUpdateException ex)
            {
                return View("Error", new ErrorViewModel { Message = ex.Message });
            }
            catch(ArgumentException ex)
            {
                return View("Error", new ErrorViewModel { Message = ex.Message });
            }
            catch(Exception ex)
            {
                return View("Error", new ErrorViewModel { Message = ex.Message });
            }
        }

    }
}
