using ExpenseTracker.Common;
using ExpenseTracker.Context;
using ExpenseTracker.Contracts;
using ExpenseTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Repository
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        
        public CategoryRepository(ExpenseTrackerDbContext db) : base(db)
        {
        }

        public async Task<PaginatedResult<Category>> GetPaginated(int page, int pageSize, string keyword)
        {
            return await GetPaginated(page, pageSize, t => t.Title.Contains(keyword ?? string.Empty));
        }
    }
}
