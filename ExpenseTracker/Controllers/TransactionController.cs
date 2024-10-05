using ExpenseTracker.Common;
using ExpenseTracker.Contracts;
using ExpenseTracker.Models;
using ExpenseTracker.ViewModel;
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
                var userId = GetUserId();

                if (!string.IsNullOrEmpty(userId))
                {
                    ViewBag.SortOrder = request.SortOrder;

                    var entities = await _repo.GetPagination(
                        request.TotalPageCount,
                        PaginatedRequest.ITEMS_PER_PAGE,
                        request.SortOrder,
                        userId, request.SearchKeyword?? string.Empty);


                    entities.SearchKeyword = request.SearchKeyword;
                    ViewBag.User = await _repo.GetUserInfo(userId);
                    return View(entities);

                }

                return NotFound("User Not Found.");
               

            }
            catch(Exception ex)
            {
                return View("Error", new ErrorViewModel { Message = ex.Message });
            }

        }

        public async Task<IActionResult> Create() 
        {

            var userId = GetUserId();

            var viewModel = new TransactionViewModel
            {
                Transaction = new Transaction(),
                Categories = await _repo.GetAllCategoriesAsync(userId)
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

                ValidateUserId(model);

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

            var userId = GetUserId(); 
          
            var entity = await _repo.GetById(id);

            if (entity == null)
                return NotFound();

            var viewModel = new TransactionViewModel()
            {
                Transaction = entity, 
                Categories = await _repo.GetAllCategoriesAsync(userId)
            };


            return View(viewModel);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(TransactionViewModel models)
        {

            try
            {
                var model = models.Transaction;

                ValidateUserId(model);

                await _repo.Update(model.TransactionId, new {model.CategoryId, model.Amount, model.Note, model.Date });

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
