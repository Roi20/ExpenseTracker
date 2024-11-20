using ExpenseTracker.Common;
using ExpenseTracker.Data;
using ExpenseTracker.Models;

namespace ExpenseTracker.Contracts
{
    public interface IAdminManageRoleRepository : IBaseRepository<AuditLog>
    {

        Task<IEnumerable<Moderator>> GetUserIsInRoleModerator();
        Task RemoveUserAsModerator(AppIdentityUser user);

    }
}
