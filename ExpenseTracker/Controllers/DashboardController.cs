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


        public IActionResult Index(DashboardViewModel model)
        {

            try
            {

                model = new DashboardViewModel
                {
                    TotalIncome = _repo.TotalIncome(),
                    TotalExpense = _repo.TotalExpense(),
                    Balance = _repo.Balance()
                    
                };

                if (model == null)
                    return NotFound("No Item Found");


                var data = _repo.GetLastTwoWeeksData()
                                .Result
                                .Where(x => x.Category.Type == "Expense")
                                .GroupBy(i => i.Category.CategoryId)
                                .Select(x => new
                                {
                                    Category = x.First().Category.Title,
                                    Sum = x.Sum(x => x.Amount),
                                    FormattedAmount = x.Sum(x => x.Amount).ToString("PHP#,##0")

                                })
                                .OrderByDescending(o => o.Sum)
                                .ToList();

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
