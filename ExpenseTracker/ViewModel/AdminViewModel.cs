﻿using ExpenseTracker.Data;
using ExpenseTracker.Models;

namespace ExpenseTracker.ViewModel
{
    public class AdminViewModel
    {
        public IEnumerable<Category> Categories { get; set; }
        public Category Category { get; set; }
        public IEnumerable<Transaction> Transactions { get; set; }
        public Transaction Transaction { get; set; }
        public IEnumerable<AppIdentityUser> Users { get; set; }
        public AppIdentityUser User { get; set; }

    }
}