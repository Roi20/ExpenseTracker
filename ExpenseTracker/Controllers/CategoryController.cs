using ExpenseTracker.Common;
using ExpenseTracker.Contracts;
using ExpenseTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Syncfusion.EJ2.Notifications;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ExpenseTracker.Controllers
{
    public class CategoryController : BaseController
    {

        private readonly ICategoryRepository _repo;

        public CategoryController(ICategoryRepository repo)
        {
            _repo = repo;
        }

        public async Task<IActionResult> Index(PaginatedRequest request)
        {
            try 
            {
                var entities = await _repo.GetPaginated(
                        request.PageNumber, 
                        PaginatedRequest.ITEMS_PER_PAGE,
                        request.SearchKeyword ??  string.Empty
                    );

                entities.SearchKeyword = request.SearchKeyword;

                return View(entities);
                 
            }
            catch
            {
                return NotFound();
            }
           
        }
        public IActionResult Create()
        {

            return View(new Category());

        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category model)
        {
            if (!ModelState.IsValid)
                return View(ModelState);

            try
            {

                await _repo.Create(model);

                TempData["Message"] = $"{model.Title}, Created Successfully";

                return Redirect("Index");

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


        public async Task<IActionResult> Update(int id) 
        {

            var entity = await _repo.GetById(id);

            if (entity == null)
            {
                return NotFound();
            }

            return View(entity);

        }
        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Category model)
        {
            if (!ModelState.IsValid) 
                return View(ModelState);
            
            try 
            {

                await _repo.Update(model.CategoryId, new {model.Title, model.Icon, model.Type });
                
                TempData["Message"] = $"{model.Title}, Updated Successfully";
                
                return RedirectToAction("Index");

            }
            catch(DbUpdateException ex) 
            {
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

        public async Task<IActionResult> ConfirmedDelete(Category model)
        {
            try
            {
                await _repo.Delete(model.CategoryId);
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
