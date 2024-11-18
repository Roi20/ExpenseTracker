using ExpenseTracker.Common;
using ExpenseTracker.Data;
using ExpenseTracker.Models;

namespace ExpenseTracker.Contracts
{
    public interface IAdminManageRoleRepository : IBaseRepository<AuditLog>
    {

        Task<IEnumerable<Moderators>> GetUserIsInRoleModerator();
        Task RemoveUserAsModerator(AppIdentityUser user);

    }
}
