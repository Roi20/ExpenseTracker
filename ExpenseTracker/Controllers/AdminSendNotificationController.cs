using ExpenseTracker.Context;
using ExpenseTracker.Contracts;
using ExpenseTracker.Data;
using ExpenseTracker.Hubs;
using ExpenseTracker.Models;
using ExpenseTracker.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging.Signing;

namespace ExpenseTracker.Controllers
{
    public class AdminSendNotificationController : AdminBaseController
    {

        private readonly INotificationRepository _repo;
        private readonly IBaseRepository<AuditLog> _baseRepo;
        private readonly UserManager<AppIdentityUser> _userManager;

        public AdminSendNotificationController(INotificationRepository repo, 
                                               IBaseRepository<AuditLog> baseRepo, 
                                               UserManager<AppIdentityUser> userManager)
        {
            _repo = repo;
            _baseRepo = baseRepo;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(AdminSendNotificationViewModel model)
        {

            model.AdminNotifications =  await _repo.GetAllAdminNotifications();

            if (model.AdminNotifications == null)
                return NotFound();

            return View(model);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateNotification(AdminSendNotificationViewModel model)
        {
            try
            {
                model.AdminNotification = new AdminNotification
                {
                    Title = model.AdminNotification.Title,
                    Message = model.AdminNotification.Message,
                };

                await _repo.SendNotificationAsync(model.AdminNotification.Title, model.AdminNotification.Message);

                TempData["SendNotificationSuccess"] = "Message sent successfully.";
                TempData["Message"] = "Message sent successfully.";

                //Create audit log
                var currentUser = await _baseRepo.GetCurrentUser();

                await _baseRepo.CreateAuditLog(currentUser.Id,
                                               currentUser.UserName ?? currentUser.Email,
                                               await _userManager.IsInRoleAsync(currentUser, "Admin") ? "Admin" : "Moderator",
                                               "Create a new notification",
                                               DateTime.UtcNow.AddHours(8),
                                               model.AdminNotification.AdminNotificationId.ToString(),
                                               "Create notification",
                                               $"Create a new notification title {model.AdminNotification.Title}");

                return RedirectToAction("Index");

            }
            catch (DbUpdateException ex)
            {

                ModelState.AddModelError("", "An error occured while trying to save your notification.");
                TempData["SendNotificationSuccess"] = "Unable to send message.";
                return View("Error", new ErrorViewModel { Message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError("", "An error occured while trying to save your notification");
                TempData["SendNotificationSuccess"] = "Unable to send message.";
                return View("Error", new ErrorViewModel { Message = ex.Message });
            }
            catch (Exception ex)
            {
                TempData["SendNotificationSuccess"] = "Unable to send message.";
                return View("Error", new ErrorViewModel { Message = ex.Message });
            }

        }

        public async Task<IActionResult> Update(int id)
        {
            try
            {
                
                
                var adminNotificationId = await _repo.GetAdminNotificationId(id);

                if (adminNotificationId == null)
                    return NotFound("Notification not found.");


                var viewModel = new AdminSendNotificationViewModel
                {
                    AdminNotification = adminNotificationId
                };
             

                return View(viewModel);

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

        [HttpPost]
        public async Task<IActionResult> UpdateNotification(AdminSendNotificationViewModel model)
        {
            try
            {

                await _repo.UpdateNotificationAsync(model.AdminNotification.AdminNotificationId, model.AdminNotification.Title, model.AdminNotification.Message);
                TempData["Message"] = "Message updated successfully.";

                //Create audit log
                var currentUser = await _baseRepo.GetCurrentUser();

                await _baseRepo.CreateAuditLog(currentUser.Id,
                                               currentUser.UserName ?? currentUser.Email,
                                               await _userManager.IsInRoleAsync(currentUser, "Admin") ? "Admin" : "Moderator",
                                               "Updated the notification",
                                               DateTime.UtcNow.AddHours(8),
                                               model.AdminNotification.AdminNotificationId.ToString(),
                                               "Update Notification",
                                               $"Update notification titled as {model.AdminNotification.Title}");

                return RedirectToAction("Index");
                 

            }
            catch (DbUpdateException ex)
            {

                ModelState.AddModelError("", "An error occured while trying to update your notification.");
                TempData["SendNotificationSuccess"] = "Unable to send message.";
                return View("Error", new ErrorViewModel { Message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError("", "An error occured while trying to update your notification");
                TempData["SendNotificationSuccess"] = "Unable to update message.";
                return View("Error", new ErrorViewModel { Message = ex.Message });
            }
            catch (Exception ex)
            {
                TempData["SendNotificationSuccess"] = "Unable to update message.";
                return View("Error", new ErrorViewModel { Message = ex.Message });
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            try
            {


                var adminNotificationId = await _repo.GetAdminNotificationId(id);

                if (adminNotificationId == null)
                    return NotFound("Notification not found.");


                var viewModel = new AdminSendNotificationViewModel
                {
                    AdminNotification = adminNotificationId
                };


                return View(viewModel);

            }
            catch (ArgumentException ex)
            {
                return View("Error", new ErrorViewModel { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { Message = ex.Message });
            }
        }


        public async Task<IActionResult> DeleteNotification(AdminSendNotificationViewModel model)
        {
            try
            {
                var notification = await _repo.GetAdminNotificationId(model.AdminNotification.AdminNotificationId);

                //Create audit log
                var currentUser = await _baseRepo.GetCurrentUser();

                await _baseRepo.CreateAuditLog(currentUser.Id,
                                               currentUser.UserName ?? currentUser.Email,
                                               await _userManager.IsInRoleAsync(currentUser, "Admin") ? "Admin" : "Moderator",
                                               "Delete Notification",
                                               DateTime.UtcNow.AddHours(8),
                                               model.AdminNotification.AdminNotificationId.ToString(),
                                               "Delete Notification",
                                               $"Delete notification titled as {notification.Title}");

                await _repo.DeleteNotificationAsync(model.AdminNotification.AdminNotificationId);
                TempData["Message"] = "Message deleted successfully.";

                return RedirectToAction("Index");


            }
            catch (DbUpdateException ex)
            {

                ModelState.AddModelError("", "An error occured while trying to save your notification.");
                TempData["SendNotificationSuccess"] = "Unable to delete message.";
                return View("Error", new ErrorViewModel { Message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError("", "An error occured while trying to delete your notification");
                TempData["SendNotificationSuccess"] = "Unable to send message.";
                return View("Error", new ErrorViewModel { Message = ex.Message });
            }
            catch (Exception ex)
            {
                TempData["SendNotificationSuccess"] = "Unable to delete message.";
                return View("Error", new ErrorViewModel { Message = ex.Message });
            }
        }


    }
}
