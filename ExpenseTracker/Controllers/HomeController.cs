using ExpenseTracker.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ExpenseTracker.Controllers
{
    public class HomeController : BaseController
    {

        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

    }
}
