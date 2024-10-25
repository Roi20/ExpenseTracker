namespace ExpenseTracker.Common
{
    public class FinancialTrendData
    {
        public string Month { get; set; }
        public double AverageIncome { get; set; }
        public double AverageExpense { get; set; }
    }


    public class ModeData()
    {

        public string Month { get; set; }
        public double ModeIncome { get; set; }
        public double ModeExpense { get; set; }
        public string Key { get; set; }
    }

    public class UserMonthlyAverage
    {
        public string UserId { get; set; }
        public string Month { get; set; }
        public double MonthlyAverageIncome { get; set; }
        public double MonthlyAverageExpense { get; set; }
    }

}
