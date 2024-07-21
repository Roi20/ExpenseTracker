using ExpenseTracker.Context;
using ExpenseTracker.Contracts;
using ExpenseTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Repository
{
    public class CategoryRepository : ICategoryRepository
    {


        public CategoryRepository(ExpenseTrackerDbContext db)
        {
            
        }

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

        public Task<Category> GetById(object id)
        {
            throw new NotImplementedException();
        }

        public Task Update(object id, object model)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Category>> UserId(string userId)
        {
            throw new NotImplementedException();
        }
    }
}
