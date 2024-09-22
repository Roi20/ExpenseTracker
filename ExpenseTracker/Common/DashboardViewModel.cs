using ExpenseTracker.Models;

namespace ExpenseTracker.Common
{
    public class DashboardViewModel
    {

        public int TotalIncome { get; set; }
        public int TotalExpense { get; set; }
        public int Balance { get; set; }
        public IEnumerable<Transaction> Transactions { get; set; }
        public LineChartData LineChartData { get; set; }
        
    }
}
