using ExpenseTracker.Common;
using ExpenseTracker.Models;
using ExpenseTracker.Repository;
using System.Linq.Expressions;

namespace ExpenseTracker.Contracts
{
    public interface ICategoryRepository : IBaseRepository<Category>
    {

        Task<PaginatedResult<Category>> GetPaginated(int page, int pageSize, string keyword, string userId);
        Task<bool> CheckIfExist(Expression<Func<Category, bool>> condition);

    }
}
