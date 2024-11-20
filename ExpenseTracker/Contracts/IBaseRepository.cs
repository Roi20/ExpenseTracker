using ExpenseTracker.Common;
using ExpenseTracker.Data;
using ExpenseTracker.Models;
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
        Task<IEnumerable<Notification>> GetAllUserNotification(string userId);
        Task<Notification> MarkAsReadUserNotification(int id);
        Task DeleteUserNotification(int id);
        Task CreateAuditLog(string userId,
                            string userName,
                            string role,
                            string action,
                            DateTime timeStamp,
                            string entityId,
                            string entityType,
                            string details);
        Task<AppIdentityUser> GetCurrentUser();

    }
}
