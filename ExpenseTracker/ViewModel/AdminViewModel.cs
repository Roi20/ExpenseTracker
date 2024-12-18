﻿using ExpenseTracker.Common;
using ExpenseTracker.Data;
using ExpenseTracker.Models;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.ViewModel
{
    public class AdminViewModel
    {
        public IEnumerable<Category> Categories { get; set; }
        public Category Category { get; set; }
        public IEnumerable<Transaction> HighestTransactions { get; set; }
        public IEnumerable<Transaction> LowestTransactions { get; set; }
        public Transaction Transaction { get; set; }
        public IEnumerable<AppIdentityUser> Users { get; set; }
        public AppIdentityUser User { get; set; }
        public int RegisteredUsersCount { get; set; }
        public int ActiveUsersCount { get; set; }
        public int InactiveUsersCount { get; set; }
        public IEnumerable<FinancialTrendData> FinancialTrendData { get; set; }
        public IEnumerable<ModeData> ModeDataSummary { get; set; }
        public IEnumerable<TopListCategories> TopListCategories { get; set; }
        public IEnumerable<Moderator> Moderators { get; set; }
        public IEnumerable<ManageAuditLog> Logs { get; set; }
        
        [FromQuery(Name = "search")]
        public string SearchKeyword { get; set; }


    }
}
