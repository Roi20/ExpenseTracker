﻿using ExpenseTracker.Common;
using ExpenseTracker.Data;
using ExpenseTracker.Models;

namespace ExpenseTracker.ViewModel
{
    public class DashboardViewModel
    {

        public int TotalIncome { get; set; }
        public int TotalExpense { get; set; }
        public int Balance { get; set; }
        public IEnumerable<Transaction> Transactions { get; set; }
        public LineChartData LineChartData { get; set; }
        public AppIdentityUser User { get; set; }
        public IEnumerable<Notification> Notifications { get; set; }
        public Notification Notification { get; set; }


    }
}
