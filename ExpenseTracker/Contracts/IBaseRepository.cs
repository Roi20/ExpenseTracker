using ExpenseTracker.Common;
using ExpenseTracker.Data;
using System.Linq.Expressions;

namespace ExpenseTracker.Contracts
{
    public interface IBaseRepository<T>
    {
        Task <IEnumerable<T>> GetAll(string userId);
        Task<T> GetById(object id);
        Task Create(T entity);
        Task Update(object id, object model);
        Task Delete(object id);
        Task <PaginatedResult<T>> GetPaginated(int page, int pageSize, Expression<Func<T, bool>> condition);
        Task<IEnumerable<T>> GetAllUserData(string userId);
        Task<AppIdentityUser> GetUserInfo(string userId);

    }
}
