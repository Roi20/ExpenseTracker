using ExpenseTracker.Common;
using ExpenseTracker.Models;
using ExpenseTracker.Repository;

namespace ExpenseTracker.Contracts
{
    public interface ITransactionRepository : IBaseRepository<Transaction>
    {

        Task <IEnumerable<Category>> GetAllCategoriesAsync(string userId);
        Task<PaginatedResult<Transaction>> GetPagination(int page, int pageSize, string sortOrder, string userId, string keyword);
        Task <IEnumerable<Transaction>> GetAllTransaction();

    }
}
