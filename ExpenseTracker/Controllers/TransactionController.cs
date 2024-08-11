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

        /*
        public async Task<IActionResult> Index()
        {
            try
            {
                var entities = await _repo.GetAll();
                return View(entities);
            }
            catch(Exception ex)
            {
                throw new Exception($"Exception: {ex.Message} || StackTrace: {ex.StackTrace}");
            }
        }
        */



        public async Task<IActionResult> Index(PaginatedRequest request) 
        {

            try
            {
                var entities = await _repo.GetPagination(
                    request.PageNumber,
                    PaginatedRequest.ITEMS_PER_PAGE
                    );

              //  entities.SearchAmount = request.SearchAmount;

                return View(entities);
            }
            catch
            {
                return NotFound();
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
                ModelState.AddModelError("", $"Unable to update todo. | Error: {ex.Message} | {ex.StackTrace}");
                return StatusCode(500, $"Unable to update database: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Exception Message: {ex.Message} | StackTrace:  {ex.StackTrace}");
            }

        }

        public IActionResult Update() 
        {

            var viewModel = new TransactionViewModel
            {
                Transaction = new Transaction(),
                Categories = _repo.GetAllCategoriesAsync().Result
            };

            return View(viewModel);
        }


        /*
        public async Task<IActionResult> Update(int id) 
        {
            
            var entity = await _repo.GetById(id);

            if (entity == null)
                return NotFound();


            return View(entity);

        }
        */


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Transaction model)
        {
            if (!ModelState.IsValid)
                return View(ModelState);

            try
            {

                await _repo.Update(model.TransactionId, new {model.CategoryId, model.Amount, model.Note, model.Date });

                TempData["Message"] = $"Transaction Updated Successfully";

                return RedirectToAction("Index");

            }
            catch (DbUpdateException ex)
            {
                throw new DbUpdateException($"DbUpdateException: Message: {ex.Message} | StackTrace: {ex.StackTrace}");
            }
            catch (Exception)
            {
                throw new Exception();
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
                throw new DbUpdateException($"DbUpdateException: Message: {ex.Message} | StackTrace: {ex.StackTrace}");
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }




    }
}
