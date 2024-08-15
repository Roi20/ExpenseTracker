using ExpenseTracker.Models;

namespace ExpenseTracker.Common
{
    public class TransactionViewModel
    {
        public Transaction Transaction { get; set; }
        public Category? Category { get; set; }
        public IEnumerable<Category> Categories { get; set; }

        public string? FormattedAmount { get; set; }
    }
}
