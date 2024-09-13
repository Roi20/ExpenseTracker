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

        public CategoryRepository(ExpenseTrackerDbContext db) : base(db)
        {
        
        }

        public async Task<bool> CheckIfExist(Expression<Func<Category, bool>> condition)
        {
            return await _table.AnyAsync(condition);
        }

        public async Task<PaginatedResult<Category>> GetPaginated(int page, int pageSize, string keyword, string userId)
        {
            return await GetPaginated(page, pageSize, 
                                      t => t.Title.Contains(keyword ?? string.Empty) 
                                      && t.User_Id == userId);
        }

    }
}
