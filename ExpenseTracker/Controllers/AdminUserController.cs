using ExpenseTracker.Common;
using ExpenseTracker.Contracts;
using ExpenseTracker.Data;
using ExpenseTracker.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ExpenseTracker.Controllers
{
    public class AdminUserController : AdminBaseController
    {

        private readonly IAdminUserRepository _repo;
        private readonly UserManager<AppIdentityUser> _userManager;

        public AdminUserController(IAdminUserRepository repo, UserManager<AppIdentityUser> userManager)
        {
            _repo = repo;
            _userManager = userManager;
        }


        public async Task<IActionResult> Index(PaginatedRequest request)
        {

            var userEntities = await _repo.GetPagination(
                               request.TotalPageCount,
                               PaginatedRequest.ITEMS_PER_PAGE,
                               request.SortOrder, 
                               request.SearchKeyword ?? string.Empty);


            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

            userEntities.SearchKeyword = request.SearchKeyword;
            ViewBag.User = await _repo.GetUserInfo(userId);
            ViewBag.SortOrder = request.SortOrder;

          
            return View(userEntities);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignRole(string userId, string role)
        {
            var user = await _repo.GetUser(userId);
            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    return NotFound("User Not Found");
                }

                if (user == null)
                {
                    return NotFound("User Not Found");
                }

                await _repo.AssignRoleAsync(user, role);
                TempData["RoleSuccessMessage"] = $"{user.FirstName} assigned as {role}";
                return RedirectToAction("Index");

            }
            catch(ArgumentException)
            {
                var isModerator = await _userManager.IsInRoleAsync(user, "Moderator");
                TempData["RoleConflictMessage"] = $"{user.FirstName} already assigned as {(isModerator ? "Moderator" : "User")}";
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                return View("Error", new ErrorViewModel { Message = ex.Message });
            }

           
        }

    }
}
