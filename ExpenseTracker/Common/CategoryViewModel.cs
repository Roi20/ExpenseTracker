using ExpenseTracker.Models;
using System.Security.Policy;

namespace ExpenseTracker.Common
{
    public class CategoryViewModel
    {
        public Category Category { get; set; }

        public IEnumerable<Category> Categories { get; set; }

    }

}
