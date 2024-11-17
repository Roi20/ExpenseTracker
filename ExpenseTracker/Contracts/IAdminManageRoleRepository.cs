using ExpenseTracker.Common;
using ExpenseTracker.Data;

namespace ExpenseTracker.Contracts
{
    public interface IAdminManageRoleRepository
    {

        Task<IEnumerable<Moderators>> GetUserIsInRoleModerator();

    }
}
