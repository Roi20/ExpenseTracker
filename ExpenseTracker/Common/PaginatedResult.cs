using ExpenseTracker.Data;

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

    }
}
