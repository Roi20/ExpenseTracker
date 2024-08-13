using ExpenseTracker.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ExpenseTracker.Controllers
{
    public class HomeController : Controller
    {


        public IActionResult Index()
        {
            return View();
        }

    }
}
