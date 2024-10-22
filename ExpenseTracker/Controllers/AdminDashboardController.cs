using ExpenseTracker.Contracts;
using ExpenseTracker.Models;
using ExpenseTracker.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ExpenseTracker.Controllers
{
    public class AdminDashboardController : AdminBaseController
    {

        private readonly IAdminDashboardRepository _repo;

        public AdminDashboardController(IAdminDashboardRepository repo)
        {
            _repo = repo;
        }

        public async Task<IActionResult> Index()
        {
            try
            {

                var model = new AdminViewModel
                {

                    RegisteredUsersCount = _repo.RegisteredUsersCount(),
                    ActiveUsersCount = _repo.ActiveUsersCount(),
                    InactiveUsersCount = _repo.InActiveUsersCount(),
                    FinancialTrendData = await _repo.GetFinancialTrendData(),


                };

               // if(model.)


                return View(model);

            }
            catch(ArgumentException ex)
            {
                return View("Error", new ErrorViewModel { Message = ex.Message });
            }
            catch(Exception ex)
            {
                // return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
                return View("Error", new ErrorViewModel { Message = ex.Message });
                //return BadRequest();
            }


        }
    }
}
