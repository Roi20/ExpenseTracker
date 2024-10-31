using ExpenseTracker.Models;

namespace ExpenseTracker.ViewModel
{
    public class TransactionViewModel
    {
        public Transaction Transaction { get; set; }
        public IEnumerable<Category> Categories { get; set; }


    }
}
