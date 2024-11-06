using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Common
{
    public class AdminNotification
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
    }
}
