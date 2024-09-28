using ExpenseTracker.Common;
using ExpenseTracker.Contracts;
using ExpenseTracker.Data;
using ExpenseTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Controllers
{
    public class UploadProfilePictureController : BaseController
    {

        private readonly IUploadRepository _repo;


        public UploadProfilePictureController(IUploadRepository repo)
        {
            _repo = repo;
        }


        public async Task<IActionResult> Index()
        {

            var currentUserId = GetUserId();

            
            ViewBag.User = await _repo.GetUser(currentUserId);
            return View();
        }


       [HttpPost]
        public async Task<IActionResult> UploadProfile(UserViewModel userModel)
        {

            var currentUserId = GetUserId();
            try
            {

                var model = userModel.ProfilePicture;
                

                ViewBag.User = await _repo.GetUser(currentUserId);
                await _repo.UploadProfilePicture(model, currentUserId);
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

                return StatusCode(500, "Oops an error occure while trying to update your profile.");
               
            }



        }
    }
}
