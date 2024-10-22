namespace ExpenseTracker.Common
{
    public class FinancialTrendData
    {
        public string Month { get; set; }
        public double AverageIncome { get; set; }
        public double AverageExpense { get; set; }
    }


    public class GetMonthData()
    {
        public int? Year { get; set; }
        public string? Month { get; set; }

    }

}
