using ExpenseTracker.Models;
using System.Security.Policy;

namespace ExpenseTracker.ViewModel
{
    public class CategoryViewModel
    {
        public Category Category { get; set; }

        public IEnumerable<Category> Categories { get; set; }

    }

}
