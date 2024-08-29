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

                var model = new DashboardViewModel
                {
                    TotalIncome = await _repo.TotalIncome(),
                    TotalExpense = await _repo.TotalExpense(),
                    Balance = await _repo.Balance()
                    
                };

                var data = await _repo.DoughnutChartData();
               

                ViewBag.DoughnutChart = Newtonsoft.Json.JsonConvert.SerializeObject(data);

                return View(model);

            }
            catch(Exception ex)
            {
                throw new Exception($"Exception Message: {ex.Message} || StackTrace: {ex.StackTrace}");
            }

       
        }
    }
}
