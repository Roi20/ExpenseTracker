using ExpenseTracker.Common;
using ExpenseTracker.Context;
using ExpenseTracker.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ExpenseTracker.Repository
{
    public class BaseRepository<T> : IBaseRepository<T>
        where T : class
    {

        private readonly DbContext _db;
        private readonly DbSet<T> _table;

        public BaseRepository(ExpenseTrackerDbContext db)
        {
            _db = db;
            _table = _db.Set<T>();

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

        public async Task<IEnumerable<T>> GetAll()
        {
            try 
            {
               return await _table.ToListAsync();
            
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

        public async Task<PaginatedResult<T>> GetPaginated(int page, int pageSize,
            Expression<Func<T, bool>> condition)
        {
            var count = await _table.Where(condition).CountAsync();

            var records = await _table.Where(condition)
                          .Skip((page - 1) * pageSize)
                          .Take(pageSize)
                          .ToListAsync();


            return new PaginatedResult<T>
            {
                Result = records,
                Page = page,
                TotalCount = (int)Math.Ceiling(count / (double)pageSize)

            };

        }


        //(Not Use) Provision for future redesign of this project with user account
        public Task<IEnumerable<T>> UserId(string userId)
        {
            throw new NotImplementedException();
        }

      
    }
}
