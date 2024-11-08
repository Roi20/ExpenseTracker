using ExpenseTracker.Context;
using ExpenseTracker.Contracts;
using ExpenseTracker.Hubs;
using ExpenseTracker.Models;
using ExpenseTracker.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging.Signing;

namespace ExpenseTracker.Controllers
{
    public class AdminSendNotificationController : AdminBaseController
    {

        private readonly INotificationRepository _repo;

        public AdminSendNotificationController(INotificationRepository repo)
        {
            _repo = repo;
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
