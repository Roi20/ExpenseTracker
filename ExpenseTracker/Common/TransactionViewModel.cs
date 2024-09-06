using ExpenseTracker.Models;

namespace ExpenseTracker.Common
{
    public class TransactionViewModel
    {
        public Transaction Transaction { get; set; }
        public IEnumerable<Category> Categories { get; set; }

    }
}
