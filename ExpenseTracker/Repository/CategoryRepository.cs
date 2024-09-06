using ExpenseTracker.Common;
using ExpenseTracker.Context;
using ExpenseTracker.Contracts;
using ExpenseTracker.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ExpenseTracker.Repository
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {

        private readonly DbContext _db;

        public CategoryRepository(ExpenseTrackerDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<bool> CheckIfExist(Expression<Func<Category, bool>> condition)
        {
            return await _table.AnyAsync(condition);
        }

        public async Task<PaginatedResult<Category>> GetPaginated(int page, int pageSize, string keyword)
        {
            return await GetPaginated(page, pageSize, t => t.Title.Contains(keyword ?? string.Empty));
        }

        public async Task CreateCategory(Category entity)
        {
            try
            {
                var checkIfExist = await CheckIfExist(x => x.Title == entity.Title);

                if (!checkIfExist)
                {

                    await Create(entity);

                }
                else
                {
                    throw new Exception("Category Title Already Exist");
                }

            }
            catch
            {
                throw;
            }
        }

        public async Task UpdateCategory(int id, Category model)
        {
            try
            {
               
                var checkIfExist = await CheckIfExist(x => x.Title == model.Title && x.CategoryId != id);

                if (!checkIfExist)
                {
                   
                    var entity = await GetById(id);
                    if(entity == null)
                    {
                        throw new Exception("Category not found.");
                    }

                    _db.Entry(entity).CurrentValues.SetValues(model);
                    await _db.SaveChangesAsync();

                }
                else
                {
                    throw new Exception("Category Title Already Exist");
                }

            }
            catch
            {
                throw;
            }
        }
    }
}
