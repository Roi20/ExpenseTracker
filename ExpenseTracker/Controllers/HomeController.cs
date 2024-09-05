using ExpenseTracker.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ExpenseTracker.Controllers
{
    public class HomeController : BaseController
    {


        public IActionResult Index()
        {
            return View();
        }

    }
}
