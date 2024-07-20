using ExpenseTracker.Models;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Controllers
{
    public class CategoryController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {

            if (!ModelState.IsValid)
                return Redirect("Index");


            return View(new Category());
        }

        public IActionResult Update()
        {
            return View();
        }

        public IActionResult Delete()
        {
            return View();
        }
    }
}
