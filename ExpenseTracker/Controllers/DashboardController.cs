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


        public async Task<IActionResult> Index()
        {

            try
            {
                //Last two weeks transactions
                var StartDate = DateOnly.FromDateTime(DateTime.Today.AddDays(-14));
                var EndDate = DateOnly.FromDateTime(DateTime.Today);

                var model = new DashboardViewModel
                {
                    TotalIncome = await _repo.TotalIncome(StartDate, EndDate),
                    TotalExpense = await _repo.TotalExpense(StartDate, EndDate),
                    Balance = await _repo.Balance(StartDate, EndDate)
                    
                };

                var LineChartData = await _repo.GetLineChartData(StartDate, EndDate);

                var data = await _repo.DoughnutChartData(StartDate, EndDate);

                ViewBag.LineChart = Newtonsoft.Json.JsonConvert.SerializeObject(LineChartData);
                ViewBag.DoughnutChart = Newtonsoft.Json.JsonConvert.SerializeObject(data);

                return View(model);

            }
            catch(Exception ex)
            {

                return View("Error", new ErrorViewModel { Message = ex.Message });

            }

       
        }
    }
}
