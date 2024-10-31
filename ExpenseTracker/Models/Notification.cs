using Microsoft.Identity.Client;

namespace ExpenseTracker.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public DateTime TimeStamp { get; set; }
        public bool IsRead { get; set; } = false;


    }
}
