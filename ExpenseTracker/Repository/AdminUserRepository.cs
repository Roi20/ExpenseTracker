using ExpenseTracker.Common;
using ExpenseTracker.Context;
using ExpenseTracker.Contracts;
using ExpenseTracker.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace ExpenseTracker.Repository
{
    public class AdminUserRepository : BaseRepository<AppIdentityUser>, IAdminUserRepository
    {


        public AdminUserRepository(ExpenseTrackerDbContext db): base(db)
        {
           
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


                case "SourceOfIncome":
                    records = records.OrderBy(i => i.SourceOfIncome);
                    break;


                case "SourceOfIncome Desc":
                    records = records.OrderByDescending(i => i.SourceOfIncome);
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
    }
}
