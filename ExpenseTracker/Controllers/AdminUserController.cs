using ExpenseTracker.Common;
using ExpenseTracker.Contracts;
using ExpenseTracker.Data;
using ExpenseTracker.Exceptions;
using ExpenseTracker.Models;
using ExpenseTracker.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            catch(RoleCountExceedException)
            {
                TempData["RoleCountExceed"] = $"The role of Moderator must be less than or equal to 5.";
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
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PaginatedResult<AppIdentityUser> model)
        {

            var userModel = model.Entity;

            try
            {

                var ifExist = await _repo.CheckIfExist(x => x.Email == userModel.Email);
                                                          
               
                if (ifExist)
                {
                    TempData["ErrorMessage"] = $"Email '{userModel.Email}' Already Exist.";
                    return RedirectToAction("Index");
                }

                await _repo.Create(userModel);

                TempData["Message"] = $"{userModel.FirstName}, Created Successfully";

                return RedirectToAction("Index");

            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError("", "An error occured while trying to save your todo item");
                return View("Error", new ErrorViewModel { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { Message = ex.Message });
            }
                
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(PaginatedResult<AppIdentityUser> user, string id)
        {

            var model = user.Entity;

            try
            {

                var ifExist = await _repo.CheckIfExist(x => x.Email == model.Email);


                if (ifExist)
                {
                    TempData["ErrorMessage"] = $"Email '{model.Email}' Already Exist.";
                    return RedirectToAction("Index");
                }

                await _repo.Update(id, new {model.Email, model.FirstName, model.LastName, model.SourceOfIncome, model.PasswordHash});

                TempData["Message"] = $"{model.FirstName}, Created Successfully";

                return RedirectToAction("Index");

            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError("", "An error occured while trying to save your todo item");
                return View("Error", new ErrorViewModel { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { Message = ex.Message });
            }


        }

        public async Task<IActionResult> ConfirmedDelete(AppIdentityUser user)
        {
            try
            {
                await _repo.Delete(user.Id);
                TempData["Message"] = $"Deleted Successfully";
                return RedirectToAction("Index");

            }
            catch (DbUpdateException ex)
            {
                return View("Error", new ErrorViewModel { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { Message = ex.Message });
            }
        }


    }
}
