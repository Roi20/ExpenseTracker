namespace ExpenseTracker.Common
{
    public class LineChartData
    {
        public string NumberOfDays { get; set; }
        public int Income { get; set; }
        public int Expense { get; set; }
        public string FormattedIncome { get; set; }
        public string FormattedExpense { get; set; }
        public int DayRange { get; set; } = 6;
    }
}
