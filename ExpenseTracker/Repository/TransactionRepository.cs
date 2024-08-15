using ExpenseTracker.Common;
using ExpenseTracker.Context;
using ExpenseTracker.Contracts;
using ExpenseTracker.Migrations;
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

        //Not in Use Method
        public async Task<PaginatedResult<Transaction>> GetPaginated(int page, int pageSize, int searchAmount)
        {
            return await GetPaginated(page, pageSize, t => t.Amount == searchAmount);
        }


        public async Task<PaginatedResult<Transaction>> GetPagination(int page, int pageSize)
        {
            var count = await _table.Include(t => t.Category).CountAsync();

            var records = await _table.Include(t => t.Category)
                      .Skip((page - 1) * pageSize)
                      .Take(pageSize)
                      .ToListAsync();


            return new PaginatedResult<Transaction>
            {
                Result = records,
                Page = page,
                TotalCount = (int)Math.Ceiling(count / (double)pageSize)

            };

        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            try
            {
                return await _categoryRepository.GetAll();
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
