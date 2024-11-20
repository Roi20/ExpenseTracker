using ExpenseTracker.Common;
using ExpenseTracker.Models;

namespace ExpenseTracker.Contracts
{
    public interface IAuditLogRepository : IBaseRepository<AuditLog>
    {
        Task<IEnumerable<ManageAuditLog>> GetAllAuditLog();

    }
}
