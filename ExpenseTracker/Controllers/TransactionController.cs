using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Controllers
{
    public class TransactionController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
