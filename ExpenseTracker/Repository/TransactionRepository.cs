using ExpenseTracker.Common;
using ExpenseTracker.Context;
using ExpenseTracker.Contracts;
using ExpenseTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Repository
{
    public class TransactionRepository : BaseRepository<Transaction>,  ITransactionRepository
    {
        public TransactionRepository(ExpenseTrackerDbContext db) : base(db)
        {
         
        }

        /*
        public async Task<PaginatedResult<Transaction>> GetPaginated(int page, int pageSize, string keyword)
        {
            return await GetPaginated(page, pageSize, t => t.Note.Contains(keyword ?? string.Empty));
        }
        */


    }
}
