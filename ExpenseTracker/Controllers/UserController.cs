using ExpenseTracker.Contracts;
using ExpenseTracker.Data;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Controllers
{
    public class UserController : BaseController
    {

        private readonly IBaseRepository<AppIdentityUser> _repo;


        public UserController(IBaseRepository<AppIdentityUser> repo)
        {
            _repo = repo;
        }

        public async Task<IActionResult> Index()
        {

            var userId = GetUserId();

            await _repo.GetAll(userId);


            return View();
        }
    }
}
