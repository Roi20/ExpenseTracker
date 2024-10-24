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

                model.ModeDataSummary = await _repo.GetModeData();

                if (model.ModeDataSummary != null)
                    return Json(model.ModeDataSummary);


                return View(model);

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
    }
}
