using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminBaseController : Controller
    {
        
    }
}
