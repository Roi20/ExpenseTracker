using ExpenseTracker.Common;
using ExpenseTracker.Context;
using ExpenseTracker.Contracts;
using ExpenseTracker.Data;
using ExpenseTracker.Exceptions;
using ExpenseTracker.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace ExpenseTracker.Repository
{
    public class AdminUserRepository : BaseRepository<AppIdentityUser>, IAdminUserRepository
    {

        private readonly UserManager<AppIdentityUser> _userManager;
        private readonly IBaseRepository<AuditLog> _baseRepo;

        public AdminUserRepository(ExpenseTrackerDbContext db, 
                                   UserManager<AppIdentityUser> userManager,
                                   IHttpContextAccessor httpContext,
                                   IBaseRepository<AuditLog> baseRepo) : base(db, httpContext, userManager)
        {
            _userManager = userManager;
            _baseRepo = baseRepo;
        }

        public async Task<PaginatedResult<AppIdentityUser>> GetPagination(int page, int pageSize, string sortOrder, string keyword)
        {

            Expression<Func<AppIdentityUser, bool>> condition = x => x.UserName.Contains(keyword ?? string.Empty) ||
                                                                     x.Email.Contains(keyword ?? string.Empty) ||
                                                                     x.FirstName.Contains(keyword ?? string.Empty) ||
                                                                     x.LastName.Contains(keyword ?? string.Empty) ||
                                                                     x.SourceOfIncome.Contains(keyword ?? string.Empty);

            var count = await _table.Where(condition).CountAsync();

            IQueryable<AppIdentityUser> records = _table.Where(condition);

            switch (sortOrder)
            {

                case "Email":
                    records = records.OrderBy(i => i.Email);
                    break;

                case "Email Desc":
                    records = records.OrderByDescending(i => i.Email);
                    break;

                case "Firstname":
                    records = records.OrderBy(i => i.FirstName);
                    break;

                case "Firstname Desc":
                    records = records.OrderByDescending(i => i.FirstName);
                    break;

                case "Lastname":
                    records = records.OrderBy(t => t.LastName);
                    break;


                case "Lastname Desc":
                    records = records.OrderByDescending(i => i.LastName);
                    break;

                default:
                    records = records.OrderBy(t => t.FirstName);
                    break;
            }

            var paginatedRecords = await records
                                        .Skip((page - 1) * pageSize)
                                        .Take(pageSize)
                                        .ToListAsync();


            return new PaginatedResult<AppIdentityUser>
            {
                Page = page,
                Result = paginatedRecords,
                TotalPage = (int)Math.Ceiling(count / (double)pageSize)

            };


        }

        public async Task<AppIdentityUser> GetUser(string userId)
        {
            return await _table.FirstOrDefaultAsync(x => x.Id == userId);
        }

        public async Task AssignRoleAsync(AppIdentityUser user, string role)
        {

            var roleCount = await _userManager.GetUsersInRoleAsync("Moderator");

            if (roleCount.Count() >= 5)
                throw new RoleCountExceedException();


            if (!await _userManager.IsInRoleAsync(user, "User") && !await _userManager.IsInRoleAsync(user, "Moderator"))
            {
                
                await _userManager.AddToRoleAsync(user, role);
                
            }
            else
            {
                throw new ArgumentException($"User already assigned as {role}");
            }

        }

        public async Task<bool> CheckIfExist(Expression<Func<AppIdentityUser, bool>> condition)
        {
            return await _table.AnyAsync(condition);
        }

        public async Task UpdateAdminPassword(string userId, AppIdentityUser adminUser)
        {

            var user = await _userManager.FindByIdAsync(userId);
            
            if (user == null && !await _userManager.IsInRoleAsync(user, "Admin"))
                throw new ArgumentException("User Not Found.");


            await _userManager.ChangePasswordAsync(user, adminUser.CurrentPassword, adminUser.Password);

        }
    }
}
