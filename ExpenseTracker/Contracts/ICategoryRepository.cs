using ExpenseTracker.Models;
using ExpenseTracker.Repository;

namespace ExpenseTracker.Contracts
{
    public interface ICategoryRepository : IBaseRepository<Category>
    {
    }
}
