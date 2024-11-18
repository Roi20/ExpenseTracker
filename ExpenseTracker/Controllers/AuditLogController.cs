using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Controllers
{
    public class AuditLogController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
