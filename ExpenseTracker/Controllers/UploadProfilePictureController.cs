using ExpenseTracker.Contracts;
using ExpenseTracker.Data;
using ExpenseTracker.Models;
using ExpenseTracker.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Controllers
{
    public class UploadProfilePictureController : BaseController
    {

        private readonly IUploadRepository _repo;
        private readonly UserManager<AppIdentityUser> _userManager;


        public UploadProfilePictureController(IUploadRepository repo, UserManager<AppIdentityUser> userManager)
        {
            _repo = repo;
            _userManager = userManager;
        }


        public async Task<IActionResult> Index()
        {

            var currentUserId = GetUserId();

            var userModel = new UserViewModel
            {
                User = await _repo.GetUser(currentUserId)
            };

            ViewBag.User = await _repo.GetUser(currentUserId);
            
            return View(userModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadProfile(UserViewModel userModel)
        {

            var currentUserId = GetUserId();
            try
            {

                var model = userModel.ProfilePicture;
                

                ViewBag.User = await _repo.GetUser(currentUserId);
                await _repo.UploadProfilePicture(model, currentUserId);
                TempData["SuccessMessage"] = "Profile";
                return RedirectToAction("Index");

            }
            catch (DbUpdateException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View("Index");
            }
            catch(ArgumentException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View("Index");

            }
            catch (Exception ex)
            {

                return StatusCode(500, "Oops an error occur while trying to update your profile.");
               
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateUserInfo(UserViewModel userModel)
        {

            try
            {
                var userId = GetUserId();

                if(userId == null)
                {
                    return StatusCode(500, "User Not Found");
                }

                ViewBag.User = await _repo.GetUser(userId);

                await _repo.UpdateUserInfo(userId, new {userModel.User.FirstName, 
                                                        userModel.User.LastName, 
                                                        userModel.User.SourceOfIncome});


                TempData["SuccessMessage"] = "Personal Information";
                return RedirectToAction("Index", "Dashboard");

            }
            catch(DbUpdateException ex)
            {
                ModelState.AddModelError("", $"Unable to update user. | Error: {ex.Message} | {ex.StackTrace}");
                return View(userModel);
            }
            catch (Exception ex)
            {
                throw new Exception($"Exception Message: {ex.Message} || StackTrace: {ex.StackTrace}");
            }


        }
    }
}
