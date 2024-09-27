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
        public async Task<IActionResult> UploadProfile(ProfilePicture model)
        {

            var currentUserId = GetUserId();
            try
            {
                ViewBag.User = await _repo.GetUser(currentUserId);
                await _repo.UploadProfilePicture(model, currentUserId);
                return RedirectToAction("Index");

            }
            catch (DbUpdateException ex)
            {
                throw new Exception($"Error Message: {ex.Message} | StackTrace: {ex.StackTrace}");
            }
            catch (Exception ex)
            {
               
                return View("Error", new ErrorViewModel { Message = ex.Message });

            }



        }
    }
}
