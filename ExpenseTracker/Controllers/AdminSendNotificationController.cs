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

        public IActionResult Index()
        {
            return View();
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
                model.Notification = new Notification
                {
                    Title = model.Notification.Title,
                    Message = model.Notification.Message,
                    IsRead = false,
                    TimeStamp = DateTime.Now,
                };

                await _repo.SendNotificationAsync(model.Notification.Title, model.Notification.Message);

                TempData["SendNotificationSuccess"] = "Message Send Successfuly.";
                return Ok("Send to user notif");

            }
            catch (DbUpdateException ex)
            {

                ModelState.AddModelError("", "An error occured while trying to save your data");
                TempData["SendNotificationSuccess"] = "Unable to send message.";
                return View("Error", new ErrorViewModel { Message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError("", "An error occured while trying to save your data");
                TempData["SendNotificationSuccess"] = "Unable to send message.";
                return View("Error", new ErrorViewModel { Message = ex.Message });
            }
            catch (Exception ex)
            {
                TempData["SendNotificationSuccess"] = "Unable to send message.";
                return View("Error", new ErrorViewModel { Message = ex.Message });
            }

        }
    }
}
