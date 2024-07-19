using ExpenseTracker.Contracts;

namespace ExpenseTracker.Repository
{
    public class BaseRepository<T> : IBaseRepository<T>
        where T : class
    {
        public Task Create(T entity)
        {
            throw new NotImplementedException();
        }

        public Task Delete(object id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<T> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task Update(object id, object model)
        {
            throw new NotImplementedException();
        }


        //(Not Use) Provision for future redesign of this project with user account
        public Task<IEnumerable<T>> UserId(string userId)
        {
            throw new NotImplementedException();
        }
    }
}
