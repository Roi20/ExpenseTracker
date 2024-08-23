using ExpenseTracker.Models;

namespace ExpenseTracker.Common
{
    public class DashboardViewModel
    {
        public DateOnly StartDate { get; private set; } = DateOnly.FromDateTime(DateTime.Today.AddDays(-14));
        public DateOnly EndDate { get; private set; } = DateOnly.FromDateTime(DateTime.Today);
        public int TotalIncome { get; set; }
        public int TotalExpense { get; set; }
        public int Balance { get; set; }
        public IEnumerable<Transaction> Transactions { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        
    }
}
