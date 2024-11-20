using ExpenseTracker.Contracts;
using ExpenseTracker.Models;
using ExpenseTracker.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Controllers
{
    public class AuditLogController : Controller
    {
        private readonly IAuditLogRepository _auditRepo;

        public AuditLogController(IAuditLogRepository auditRepo)
        {
            _auditRepo = auditRepo;
        }

        public async Task<IActionResult> Index(AdminViewModel viewModel)
        {

            try
            {
                viewModel.Logs = await _auditRepo.GetAllAuditLog();

                return View(viewModel);

            }
            catch(FormatException ex)
            {
                return View("Error", new ErrorViewModel { Message = ex.Message });
            }
            catch(Exception ex)
            {
                return View("Error", new ErrorViewModel { Message = ex.Message });
            }


           
        }
    }
}
