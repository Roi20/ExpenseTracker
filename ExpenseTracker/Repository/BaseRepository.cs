using ExpenseTracker.Common;
using ExpenseTracker.Context;
using ExpenseTracker.Contracts;
using ExpenseTracker.Data;
using ExpenseTracker.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Syncfusion.EJ2.Linq;
using System.Linq.Expressions;

namespace ExpenseTracker.Repository
{
    public class BaseRepository<T> : IBaseRepository<T>
        where T : class, IBaseModel
    {

        private readonly DbContext _db;
        protected readonly DbSet<T> _table;
        private readonly DbSet<AppIdentityUser> _user;
        protected readonly DbSet<Notification> _notification;

        public BaseRepository(ExpenseTrackerDbContext db)
        {
            _db = db;
            _table = _db.Set<T>();
            _user = _db.Set<AppIdentityUser>();
            _notification = _db.Set<Notification>();

        }

        public async Task<T> GetById(object id)
        {
            try
            {

                return await _table.FindAsync(id);

            }
            catch
            {
                throw new Exception("An error occured while trying to fetch data.");
            }
        }

        public async Task Create(T entity)
        {
            try 
            {
                await _table.AddAsync(entity);
                await _db.SaveChangesAsync();
            }
            catch(DbUpdateException)
            {
                throw;
            }
            catch(Exception)
            {
                throw;
            }
        }

        public async Task Delete(object id)
        {
            try
            {
                var entity = await GetById(id);

                if(entity != null) 
                {
                    _table.Remove(entity);
                    await _db.SaveChangesAsync();
                }

            }
            catch(DbUpdateException)
            {
                throw;
            }
        }

        public async Task<IEnumerable<T>> GetAll(string userId)
        {
            try 
            {
               return await _table.Where(x => x.User_Id == userId).ToListAsync();
            
            }
            catch (Exception) 
            {
                throw;
            }
        }

        public async Task Update(object id, object model)
        {
            try 
            {
                var entity = await GetById(id);

                if(entity != null) 
                {
                    _db.Entry(entity).CurrentValues.SetValues(model);
                    await _db.SaveChangesAsync();
                }

            }
            catch (DbUpdateException) 
            {
                throw;
            }
            catch (Exception) 
            {
                throw;
            }
        }

        public async Task<PaginatedResult<T>> GetPaginated(int page, 
                                                           int pageSize, 
                                                           Expression<Func<T, bool>> condition)
        {
            var totalCount = await _table.Where(condition).CountAsync();

            var records = await _table.Where(condition)
                                      .Skip((page - 1) * pageSize)
                                      .Take(pageSize)
                                      .ToListAsync();

            return new PaginatedResult<T>
            {
                Result = records,
                Page = page,
                TotalPage = (int)Math.Ceiling(totalCount / (double)pageSize)
            };
       
              

        }

        public async Task<IEnumerable<T>> GetAllUserData(string userId)
        {
            return await _table.Where(x => x.User_Id == userId).ToListAsync();
        }

        public async Task<AppIdentityUser> GetUserInfo(string userId)
        {
            return await _user.FirstOrDefaultAsync(x => x.Id == userId);
        }

        public async Task<IEnumerable<Notification>> GetAllUserNotification(string userId)
        {
            return await _notification.Where(x => x.UserId == userId).ToListAsync();
        }
        public async Task<Notification> MarkAsReadUserNotification(int id)
        {
            try
            {
                var notificationId = await _notification.FindAsync(id);

                if (notificationId == null)
                    throw new ArgumentException("Notification not found.");


                notificationId.IsRead = true;
                await _db.SaveChangesAsync();

                return notificationId;


            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}
