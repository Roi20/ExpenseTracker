namespace ExpenseTracker.Common
{
    public class PaginatedResult<T>
    {
        public int Page { get; set; }
        public int TotalCount { get; set; }
        public string? SearchKeyword { get; set; }
        public IEnumerable<T>? Result { get; set; }

    }
}
