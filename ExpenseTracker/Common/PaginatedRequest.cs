using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Common
{
    public class PaginatedRequest
    {

        public const int ITEMS_PER_PAGE = 2;

        [FromQuery(Name = "p")]
        public int PageNumber { get; set; } = 1;

        [FromQuery(Name = "s")]
        public string? SearchKeyword { get; set; }

        public string SortOrder { get; set; }
    }
}
