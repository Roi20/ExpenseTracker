using ExpenseTracker.Common;
using ExpenseTracker.Context;
using ExpenseTracker.Contracts;
using ExpenseTracker.Data;
using ExpenseTracker.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Syncfusion.EJ2.Linq;
using System.Linq.Expressions;

namespace ExpenseTracker.Repository
{
    public class AuditLogRepository : BaseRepository<AuditLog>, IAuditLogRepository
    {
        private readonly IBaseRepository<AuditLog> _baseRepo;

        public AuditLogRepository(ExpenseTrackerDbContext db, 
                                  IHttpContextAccessor httpContext, 
                                  UserManager<AppIdentityUser> userManager,
                                  IBaseRepository<AuditLog> baseRepo) : base(db, httpContext, userManager)
        {
            _baseRepo = baseRepo;
        }

        public async Task<IEnumerable<ManageAuditLog>> GetAllAuditLog(string searchKeyword)
        {
            try
            {

                var records = await _table.ToListAsync();

                if (!string.IsNullOrEmpty(searchKeyword))
                {
                    var normalizeKeyword = searchKeyword.ToLower();

                    records = records.Where(x => x.UserName.ToLower().Contains(normalizeKeyword) ||
                                                                     x.Role.ToLower().Contains(normalizeKeyword) ||
                                                                     x.Action.ToLower().Contains(normalizeKeyword) ||
                                                                     x.Details.ToLower().Contains(normalizeKeyword) ||
                                                                     x.TimeStamp.ToString("g").ToLower().Contains(normalizeKeyword))
                                                                     .ToList();
                }
                                               

                var auditLogs =   records.OrderByDescending(x => x.TimeStamp)
                                         .Select(s => new ManageAuditLog
                                         {
                                             UserId = s.User_Id,
                                             Username = s.UserName,
                                             Role = s.Role,
                                             Action = s.Action,
                                             Details = s.Details,
                                             TimeStamp = s.TimeStamp.ToString("g")

                                         }).ToList();


                return auditLogs;
            }
            catch (FormatException ex)
            {
                throw new InvalidOperationException("There was an error parsing the date format of the audit logs.", ex);
            }
            catch (Exception ex) 
            {
                throw new InvalidOperationException("An unexpected error occurred while retrieving audit logs.", ex);
            }
        }
    }
}
