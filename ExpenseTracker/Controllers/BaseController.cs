using ExpenseTracker.Contracts;
using ExpenseTracker.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ExpenseTracker.Controllers
{

    [Authorize]
    public class BaseController : Controller
    {
        protected string GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
        }


        protected void ValidateUserId<T>(T model)
            where T : IBaseModel
        {
            var userId = GetUserId();

            if (!string.IsNullOrEmpty(userId))
            {
                model.User_Id = userId;
            }
        }
    }
}
