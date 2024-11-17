using ExpenseTracker.Contracts;
using ExpenseTracker.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Controllers
{
    public class AdminManageRoleController : AdminBaseController
    {

        private readonly IAdminManageRoleRepository _repo;

        public AdminManageRoleController(IAdminManageRoleRepository repo)
        {
            _repo = repo;
        }

        public async Task<IActionResult> Index(AdminViewModel model)
        {
            model.Moderators = await _repo.GetUserIsInRoleModerator();

            return View(model);
        }
    }
}
