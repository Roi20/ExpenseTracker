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

        public async Task<PaginatedResult<Category>> GetPaginated(int page, 
                                                                  int pageSize, 
                                                                  string keyword, 
                                                                  string userId, 
                                                                  string sortOrder)
        {
            var totalCount = await _table.Where(x => x.Title.Contains(keyword ?? string.Empty)).CountAsync();

            var records = _table.Where(x => x.Title.Contains(keyword?? string.Empty) && 
                                            x.User_Id == userId);



            switch (sortOrder)
            {
                case "Icon":
                    records = records.OrderBy(x => x.Icon);
                    break;
                case "Icon_Desc":
                    records = records.OrderByDescending(x => x.Icon);
                    break;

                case "Title":
                    records = records.OrderBy(x => x.Title);
                    break;
                case "Title_Desc":
                    records = records.OrderByDescending(x => x.Title);
                    break;

                case "Type":
                    records = records.OrderBy(x => x.Type);
                    break;
                case "Type_Desc":
                    records = records.OrderByDescending(x => x.Type);
                    break;

                default:
                    records = records.OrderBy(x => x.CategoryId);
                    break;
                
                   
            }


            var paginatedRecords = await records.Skip((page - 1) * pageSize)
                                                .Take(pageSize)
                                                .ToListAsync();


            return new PaginatedResult<Category>
            {
                Result = paginatedRecords,
                Page = page,
                TotalPage = (int)Math.Ceiling(totalCount / (double)pageSize)
            };



        }

    }
}
