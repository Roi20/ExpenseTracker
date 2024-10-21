using ExpenseTracker.Contracts;
using ExpenseTracker.Models;
using ExpenseTracker.ViewModel;
using Microsoft.AspNetCore.Mvc;

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


                };

                return View(model);

            }
            catch(Exception ex)
            {
                return View("Error", new ErrorViewModel { Message = ex.Message });
            }


        }
    }
}
