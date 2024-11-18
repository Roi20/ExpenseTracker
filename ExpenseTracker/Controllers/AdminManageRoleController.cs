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

        public AdminManageRoleController(IAdminManageRoleRepository repo, UserManager<AppIdentityUser> userManager)
        {
            _repo = repo;
            _userManager = userManager;
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

                return RedirectToAction("Index");

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
