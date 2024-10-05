using ExpenseTracker.Common;
using ExpenseTracker.Contracts;
using ExpenseTracker.Data;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ExpenseTracker.Controllers
{
    public class AdminUserController : AdminBaseController
    {

        private readonly IAdminUserRepository _repo;

        public AdminUserController(IAdminUserRepository repo)
        {
            _repo = repo;
        }


        public async Task<IActionResult> Index(PaginatedRequest request)
        {



            var userEntities = await _repo.GetPagination(
                               request.TotalPageCount,
                               PaginatedRequest.ITEMS_PER_PAGE,
                               request.SortOrder, 
                               request.SearchKeyword ?? string.Empty);


            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

            userEntities.SearchKeyword = request.SearchKeyword;
            ViewBag.User = await _repo.GetUserInfo(userId);
            ViewBag.SortOrder = request.SortOrder;

          
            return View(userEntities);


        }
    }
}
