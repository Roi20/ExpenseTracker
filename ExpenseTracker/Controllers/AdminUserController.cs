using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Controllers
{
    public class AdminUserController : AdminBaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
