using ExpenseTracker.Common;
using ExpenseTracker.Data;
using ExpenseTracker.Models;
using ExpenseTracker.ViewModel;
using System.Linq.Expressions;

namespace ExpenseTracker.Contracts
{
    public interface IAdminUserRepository : IBaseRepository<AppIdentityUser>
    {

        Task<PaginatedResult<AppIdentityUser>> GetPagination(int page, int pageSize, string sortOrder, string keyword);

        Task<AppIdentityUser> GetUser(string userId);

        Task AssignRoleAsync(AppIdentityUser user, string role);

        Task<bool> CheckIfExist(Expression<Func<AppIdentityUser, bool>> condition);

    }
}
