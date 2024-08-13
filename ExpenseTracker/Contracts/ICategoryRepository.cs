using ExpenseTracker.Common;
using ExpenseTracker.Models;
using ExpenseTracker.Repository;

namespace ExpenseTracker.Contracts
{
    public interface ICategoryRepository : IBaseRepository<Category>
    {

        Task<PaginatedResult<Category>> GetPaginated(int page, int pageSize, string keyword);

    }
}
