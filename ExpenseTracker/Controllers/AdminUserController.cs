using ExpenseTracker.Common;
using ExpenseTracker.Contracts;
using ExpenseTracker.Data;
using ExpenseTracker.Exceptions;
using ExpenseTracker.Models;
using ExpenseTracker.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Syncfusion.EJ2.Linq;
using System.Security.Claims;

namespace ExpenseTracker.Controllers
{
    public class AdminUserController : AdminBaseController
    {

        private readonly IAdminUserRepository _repo;
        private readonly UserManager<AppIdentityUser> _userManager;
        private readonly IBaseRepository<AuditLog> _baseRepo;

        public AdminUserController(IAdminUserRepository repo, UserManager<AppIdentityUser> userManager, IBaseRepository<AuditLog> baseRepo)
        {
            _repo = repo;
            _userManager = userManager;
            _baseRepo = baseRepo;
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
            userEntities.User = await _repo.GetUserInfo(userId);
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
                    return NotFound("User Id cannot be null or empty.");
                }

                if(user == null)
                {
                    return NotFound("User not found");
                }


                await _repo.AssignRoleAsync(user, role);
                TempData["RoleSuccessMessage"] = $"{user.FirstName} assigned as {role}";

                var currentUser = await _baseRepo.GetCurrentUser();

                await _baseRepo.CreateAuditLog(currentUser.Id,
                                              currentUser.UserName ?? currentUser.Email,
                                              await _userManager.IsInRoleAsync(currentUser, "Admin") ? "Admin" : "Moderator",
                                              $"Add user role as {role}.",
                                              DateTime.UtcNow.AddHours(8),
                                              user.Id,
                                              "User Role",
                                              $"Add {user.UserName ?? user.Email} as {role}");



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

                var ifExist = await _userManager.FindByEmailAsync(userModel.Email);
                                                          
                if (ifExist != null)
                {
                    TempData["ErrorMessage"] = $@"Email: {userModel.Email} Already Exist.";
                    return RedirectToAction("Index");
                }

                userModel.EmailConfirmed = true;
                userModel.UserName = model.Entity.Email;
                userModel.NormalizedEmail = model.Entity.Email;
                userModel.NormalizedUserName = model.Entity.Email;

                await _userManager.CreateAsync(userModel, userModel.Password);

                TempData["Message"] = $"{userModel.Email}, Created Successfully";

                //Create Audit Logs
                var currentUser = await _baseRepo.GetCurrentUser();

                await _baseRepo.CreateAuditLog(currentUser.Id,
                                               currentUser.UserName ?? currentUser.Email,
                                               await _userManager.IsInRoleAsync(currentUser, "Admin") ? "Admin" : "Moderator",
                                               $"Create new user",
                                               DateTime.UtcNow.AddHours(8),
                                               userModel.Id,
                                               "Creating User",
                                               $"Created a new user {userModel.UserName}");

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
        public async Task<IActionResult> Update(PaginatedResult<AppIdentityUser> model)
        {
            
            var userModel = model.Entity;

            try
            {
                var user = await _userManager.FindByIdAsync(userModel.Id);

                if (user == null)
                    return NotFound();

                user.Email = userModel.Email;
                user.FirstName = userModel.FirstName;
                user.LastName = userModel.LastName;
                user.SourceOfIncome = userModel.SourceOfIncome;
                user.Password = userModel.Password;
                user.UserName = userModel.Email;
                user.NormalizedEmail = userModel.Email;
                user.NormalizedUserName = userModel.Email;
                user.EmailConfirmed = true;

                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, token, userModel.Password);


                if (result.Succeeded)
                {
                    TempData["Message"] = $"{userModel.Email}, Updated Successfully";

                    //Create Audit Logs
                    var currentUser = await _baseRepo.GetCurrentUser();

                    await _baseRepo.CreateAuditLog(currentUser.Id,
                                                   currentUser.UserName ?? currentUser.Email,
                                                   await _userManager.IsInRoleAsync(currentUser, "Admin") ? "Admin" : "Moderator",
                                                   $"Update user",
                                                   DateTime.UtcNow.AddHours(8),
                                                   userModel.Id,
                                                   "Updating User",
                                                   $"Updated user {user.UserName} information.");

                    return RedirectToAction("Index");
                }
                else
                {

                    TempData["ResultErrorMessage"] = result.Errors.FirstOrDefault()?.Description.ToString();

                    return RedirectToAction("Update");
                }                 

            }
            catch (Exception ex)
            {
                _userManager.Logger.LogError(ex, "Unable to update User");
                ModelState.AddModelError(string.Empty, "An unexpected error occurred. Please try again.");
                return View(model);
            }
            
        }
   
        public async Task<IActionResult> Update(string id)
        {

            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
                return NotFound();

            var userModel = new PaginatedResult<AppIdentityUser> 
            {
               Entity = user
            };

            return View(userModel);
       
        }

        public async Task<IActionResult> Delete(string id)
        {

            

            var user = await _userManager.FindByIdAsync(id);

          
            if (user == null)
                return NotFound();

            if (await _userManager.IsInRoleAsync(user, "Admin"))
            {
                TempData["AdminMessage"] = "Unable to delete user, You are trying to delete a user with admin role.";
                return RedirectToAction("Index");
            }
                

            var userModel = new PaginatedResult<AppIdentityUser>
            {
                Entity = user
            };

            return View(userModel);
        }


        [HttpPost]
        public async Task<IActionResult> ConfirmedDelete(PaginatedResult<AppIdentityUser> model)
        {

            var userModel = model.Entity;

            try
            {
                var user = await _userManager.FindByIdAsync(userModel.Id);

                if (user == null)
                    return NotFound();

                

                //Create Audit Logs
                var currentUser = await _baseRepo.GetCurrentUser();

                await _baseRepo.CreateAuditLog(currentUser.Id,
                                               currentUser.UserName ?? currentUser.Email,
                                               await _userManager.IsInRoleAsync(currentUser, "Admin") ? "Admin" : "Moderator",
                                               $"Delete user",
                                               DateTime.UtcNow.AddHours(8),
                                               userModel.Id,
                                               "Deleting User",
                                               $"Deleted user {user.UserName}");

                await _userManager.DeleteAsync(user);
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

        public async Task<IActionResult> AdminManagePassword(AdminViewModel model)
        {

            try
            {
                var userId = GetUserId();

                var user = await _userManager.FindByIdAsync(userId);

                if (!await _userManager.IsInRoleAsync(user, "Admin"))
                {
                    throw new UnauthorizedAccessException("User is not an admin");
                }

                model.User = user;

                return View(model);

            }
            catch (UnauthorizedAccessException ex)
            {
                return View("Error", new ErrorViewModel { Message = ex.Message });
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

        [HttpPost]
        public async Task<IActionResult> UpdateAdminPassword(AdminViewModel model)
        {

            try
            {
                var userId = GetUserId();

                if (model.User.ConfirmPassword != model.User.Password)
                {
                    TempData["ConfirmPasswordMessage"] = "Password and Confirm Password does not match";
                    return RedirectToAction("AdminManagePassword");
                }
  
                await _repo.UpdateAdminPassword(userId, model.User);

                TempData["Message"] = "Admin password updated";

                //Create Audit Logs
                var currentUser = await _baseRepo.GetCurrentUser();

                await _baseRepo.CreateAuditLog(currentUser.Id,
                                            currentUser.UserName ?? currentUser.Email,
                                            await _userManager.IsInRoleAsync(currentUser, "Admin") ? "Admin" : "Moderator",
                                            $"Update admin password.",
                                            DateTime.UtcNow.AddHours(8),
                                            model.User.Id,
                                            "Admin Password",
                                            $"Updated the admin password");


                return RedirectToAction("Index");

            }
            catch(ArgumentException ex)
            {
                return View("Error", new ErrorViewModel { Message = ex.Message });
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
