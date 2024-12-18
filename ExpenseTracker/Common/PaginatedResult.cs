﻿using ExpenseTracker.Data;
using ExpenseTracker.Models;

namespace ExpenseTracker.Common
{
    public class PaginatedResult<T>
    {
        public int Page { get; set; }
        public int TotalPage { get; set; }
        public string? SearchKeyword { get; set; }
        public string? SortOrder { get; set; }
        public int? SearchAmount { get; set; }
        public IEnumerable<T>? Result { get; set; }
        public T Entity { get; set; }
        public AppIdentityUser User { get; set; }
        public IEnumerable<Notification> Notifications { get; set; }

    }
}
