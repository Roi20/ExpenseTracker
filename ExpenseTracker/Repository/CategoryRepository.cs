using ExpenseTracker.Contracts;
using ExpenseTracker.Models;

namespace ExpenseTracker.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        public Task Create(Category entity)
        {
            throw new NotImplementedException();
        }

        public Task Delete(object id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Category>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Category> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task Update(object id, object model)
        {
            throw new NotImplementedException();
        }

        //provision
        public Task<IEnumerable<Category>> UserId(string userId)
        {
            throw new NotImplementedException();
        }
    }
}
