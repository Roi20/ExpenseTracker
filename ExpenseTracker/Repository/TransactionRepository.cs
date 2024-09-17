using ExpenseTracker.Common;
using ExpenseTracker.Context;
using ExpenseTracker.Contracts;
//using ExpenseTracker.Migrations;
using ExpenseTracker.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Repository
{
    public class TransactionRepository : BaseRepository<Transaction>,  ITransactionRepository
    {

        private readonly ICategoryRepository _categoryRepository;
        private readonly DbContext _db;


        public TransactionRepository(ExpenseTrackerDbContext db, ICategoryRepository categoryRepository) : base(db)
        {
            _categoryRepository = categoryRepository;
            _db = db;


        }

        public async Task<PaginatedResult<Transaction>> GetPagination(int page, int pageSize, string sortOrder, string userId)
        {
            var count = await _table.Include(t => t.Category).CountAsync();

            IQueryable<Transaction> records = _table.Where(x => x.User_Id == userId)
                                                    .Include(i => i.Category);
                                                   


            switch (sortOrder)
            {
                case "Category":
                    records = records.OrderBy(i => i.Category.Title);
                    break;

                case "Category Desc":
                    records = records.OrderByDescending(i => i.Category.Title);
                    break;

                case "Amount":
                    records = records.OrderBy(i => i.Amount);
                    break;

                case "Amount Desc":
                    records = records.OrderByDescending(i => i.Amount);
                    break;

                case "Date Desc":
                    records = records.OrderByDescending(i => i.Date);
                    break;

                default:
                    records = records.OrderBy(t => t.Date);
                    break;
            }
                      

            var paginatedRecords = await records
                                         .Skip((page - 1) * pageSize)
                                         .Take(pageSize)
                                         .ToListAsync();

            return new PaginatedResult<Transaction>
            {
                Result = paginatedRecords,
                Page = page,
                TotalPage = (int)Math.Ceiling(count / (double)pageSize)

            };

        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync(string userId)
        {
            try
            {
                return await _categoryRepository.GetAll(userId);
            }
            catch (Exception) 
            {
                throw;
            }
            
        }

        public async Task<IEnumerable<Transaction>> GetAllTransaction()
        {
            return await _table.Include(t => t.Category).ToListAsync();
           
        }



    }
}
