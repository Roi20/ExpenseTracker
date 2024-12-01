using ExpenseTracker.Contracts;
using ExpenseTracker.Data;
using ExpenseTracker.Hubs;
using ExpenseTracker.Models;
using ExpenseTracker.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace ExpenseTracker.Controllers
{
    public class DashboardController : BaseController
    {

        private readonly IDashboardRepository _repo;
        private readonly UserManager<AppIdentityUser> _userManager;
        private readonly IHubContext<NotificationHub, INotificationHub> _hub;

        public DashboardController(IDashboardRepository repo, UserManager<AppIdentityUser> userManager, IHubContext<NotificationHub, INotificationHub> hub)
        {
            _repo = repo;
            _userManager = userManager;
            _hub = hub;
           
        }
        public async Task<IActionResult> Index(int dayRange = 7)
        {

            var currentUserId = GetUserId();

            try
            {

                var StartDate = DateOnly.FromDateTime(DateTime.Today.AddDays(-dayRange));
                var EndDate = DateOnly.FromDateTime(DateTime.Today);

                var model = new DashboardViewModel
                {
                    TotalIncome = await _repo.TotalIncome(StartDate, EndDate, currentUserId),
                    TotalExpense = await _repo.TotalExpense(StartDate, EndDate, currentUserId),
                    Balance = await _repo.Balance(StartDate, EndDate, currentUserId),
                    Transactions = await _repo.GetAllTransaction(currentUserId),
                    User = await _repo.GetUserInfo(currentUserId),
                    Notifications = await _repo.GetAllUserNotification(currentUserId)
                };
            
                var LineChartData = await _repo.GetLineChartData(StartDate, EndDate,  dayRange + 1, currentUserId);

                var data = await _repo.DoughnutChartData(StartDate, EndDate, currentUserId);
                ViewBag.LineChart = Newtonsoft.Json.JsonConvert.SerializeObject(LineChartData);
                ViewBag.DoughnutChart = Newtonsoft.Json.JsonConvert.SerializeObject(data);
                ViewBag.DayRange = dayRange;
                ViewBag.User = await _repo.GetUserInfo(currentUserId);
                return View(model);

            }
            catch(Exception ex)
            {

                return View("Error", new ErrorViewModel { Message = ex.Message });

            }

       
        }

        [HttpPost]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            try
            {
                var userNotificationId = await _repo.MarkAsReadUserNotification(id);

                if (userNotificationId == null)
                    return NotFound("Notification not found.");

                return Ok();

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

        [HttpPost]
        public async Task<IActionResult> ConfirmedDeleteNotification(int id)
        {
            try
            {
                await _repo.DeleteUserNotification(id);


                return Ok();

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

    }
}
