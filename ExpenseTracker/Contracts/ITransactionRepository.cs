using ExpenseTracker.Common;
using ExpenseTracker.Models;
using ExpenseTracker.Repository;

namespace ExpenseTracker.Contracts
{
    public interface ITransactionRepository : IBaseRepository<Transaction>
    {

        Task <IEnumerable<Category>> GetAllCategoriesAsync();
        Task<PaginatedResult<Transaction>> GetPaginated(int page, int pageSize, int searchAmount);
        Task<PaginatedResult<Transaction>> GetPagination(int page, int pageSize, string sortOrder);
        Task <IEnumerable<Transaction>> GetAllTransaction();

    }
}
