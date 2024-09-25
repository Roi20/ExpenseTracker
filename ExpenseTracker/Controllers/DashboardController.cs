using ExpenseTracker.Common;
using ExpenseTracker.Contracts;
using ExpenseTracker.Models;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Controllers
{
    public class DashboardController : BaseController
    {

        private readonly IDashboardRepository _repo;
      
        public DashboardController(IDashboardRepository repo)
        {
            _repo = repo;
           
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






    }
}
