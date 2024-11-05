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
        private readonly IHubContext<NotificationHub, INotificationHub> _hubContext;
        private readonly DbContext _db;
        private readonly DbSet<Notification> _notif;
        public AdminSendNotificationController(IHubContext<NotificationHub, INotificationHub> hubContext, ExpenseTrackerDbContext db)
        {
            _hubContext = hubContext;
            _db = db;
            _notif = _db.Set<Notification>();
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
                    //UserId = userId,
                    Title = model.Notification.Title,
                    Message = model.Notification.Message,
                    IsRead = false,
                    TimeStamp = DateTime.Now,
                };

                _notif.Add(model.Notification);
                await _db.SaveChangesAsync();
                await _hubContext.Clients.All.ReceiveNotification(model.Notification.Title, model.Notification.Message, model.Notification.TimeStamp, model.Notification.IsRead);
                TempData["SendNotificationSuccess"] = "Message Send Successfuly.";
                return RedirectToAction("Create");

            }
            catch (DbUpdateException ex)
            {

                ModelState.AddModelError("", "An error occured while trying to save your todo item");
                TempData["SendNotificationSuccess"] = "Unable to send message.";
                return View("Error", new ErrorViewModel { Message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError("", "An error occured while trying to save your todo item");
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
