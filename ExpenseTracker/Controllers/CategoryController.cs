using ExpenseTracker.Common;
using ExpenseTracker.Contracts;
using ExpenseTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.CodeAnalysis.CSharp.Syntax;
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
                var userId = GetUserId();

                if(userId != string.Empty )
                {
                    var entities = await _repo.GetPaginated(
                     request.PageNumber,
                     PaginatedRequest.ITEMS_PER_PAGE,
                     request.SearchKeyword ?? string.Empty,
                     userId
                    );

                    entities.SearchKeyword = request.SearchKeyword;

                    return View(entities);
                }

                return NotFound("User Not Found");


            }
            catch
            {
                return NotFound();
            }
           
        }
        public IActionResult Create()
        {

            var viewModel = new CategoryViewModel()
            {
                Category = new Category()
            };

            return View(viewModel);

        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryViewModel model)
        {

            var viewModel = model.Category;

            ValidateUserId(viewModel);
            var currentUserId = GetUserId();
            try
            {

                var ifExist = await _repo.CheckIfExist(x => x.Title == viewModel.Title && 
                                                            x.User_Id == currentUserId);

                if (ifExist)
                {
                    TempData["ErrorMessage"] = "Category Already Exist.";
                    return View();
                }
                    
                await _repo.Create(viewModel);

                TempData["Message"] = $"{viewModel.Title}, Created Successfully";

                return Redirect("Index");

            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError("", "An error occured while trying to save your todo item");
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

            var viewModel = new CategoryViewModel()
            {
                Category = entity
            };

            return View(viewModel);

        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(CategoryViewModel model)
        {

            var viewModel = model.Category;

            ValidateUserId(viewModel);
            var currentUserId = GetUserId();
            try 
            {

                var ifExist = await _repo.CheckIfExist(x => x.Title == viewModel.Title &&
                                                            x.User_Id == currentUserId);

                if (ifExist)
                {
                    TempData["ErrorMessage"] = "Category Already Exist.";
                    return View(model);
                }

                await _repo.Update(viewModel.CategoryId, new {viewModel.Title, viewModel.Icon, viewModel.Type});
                
                TempData["Message"] = $"{viewModel.Title}, Updated Successfully";
                
                return RedirectToAction("Index");

            }
            catch(DbUpdateException ex) 
            {
                ModelState.AddModelError("", "An error occured while trying to save your todo item");
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
