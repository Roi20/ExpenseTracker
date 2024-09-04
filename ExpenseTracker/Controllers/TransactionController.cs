using ExpenseTracker.Common;
using ExpenseTracker.Contracts;
using ExpenseTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace ExpenseTracker.Controllers
{
    public class TransactionController : BaseController
    {

        private readonly ITransactionRepository _repo;

        public TransactionController(ITransactionRepository repo)
        {
            _repo = repo;            
        }


        public async Task<IActionResult> Index(PaginatedRequest request)
        {

            try
            {

                ViewBag.SortOrder = request.SortOrder;

                var entities = await _repo.GetPagination(
                    request.PageNumber,
                    PaginatedRequest.ITEMS_PER_PAGE,
                    request.SortOrder);

                return View(entities);

            }
            catch(Exception ex)
            {
                return View("Error", new ErrorViewModel { Message = ex.Message });
            }

        }

        public IActionResult Create() 
        {
            var viewModel = new TransactionViewModel
            {
                Transaction = new Transaction(),
                Categories = _repo.GetAllCategoriesAsync().Result
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TransactionViewModel models)
        {

            try
            {
                var model = models.Transaction;

                await _repo.Create(model);

                TempData["Message"] = $"New Transaction Created Successfully";

                return RedirectToAction("Index");

            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError("", $"Unable to update | Error: {ex.Message} | {ex.StackTrace}");
                return View("Error", new ErrorViewModel { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { Message = ex.Message });
            }

        }
        public async Task<IActionResult> Update(int id) 
        {
          
            var entity = await _repo.GetById(id);

            if (entity == null)
                return NotFound();

            var viewModel = new TransactionViewModel()
            {
                Transaction = entity, 
                Categories = _repo.GetAllCategoriesAsync().Result
            };


            return View(viewModel);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(TransactionViewModel model)
        {

            try
            {

                var entity = model.Transaction;

                await _repo.Update(entity.TransactionId, new {entity.CategoryId, entity.Amount, entity.Note, entity.Date });

                TempData["Message"] = $"Transaction Updated Successfully";

                return RedirectToAction("Index");

            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError("", $"Unable to update | Error: {ex.Message} | {ex.StackTrace}");
                return View("Error", new ErrorViewModel { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { Message = ex.Message });
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _repo.GetById(id);

            if (entity == null)
            {
                return NotFound();
            }

            

            return View(entity);
        }

        public async Task<IActionResult> ConfirmedDelete(Transaction model)
        {
            try
            {
                await _repo.Delete(model.TransactionId);
                TempData["Message"] = $"Deleted Successfully";
                return RedirectToAction("Index");

            }
            catch (DbUpdateException ex)
            {
                return View("Error", new ErrorViewModel { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { Message = ex.Message });
            }
        }




    }
}
