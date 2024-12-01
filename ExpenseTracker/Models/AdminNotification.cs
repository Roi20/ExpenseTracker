using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Models
{
    public class AdminNotification
    {
        [Key]
        public int AdminNotificationId { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public DateTime TimeStamp { get; set; } = DateTime.Now;
        public virtual ICollection<Notification> Notifications { get; set; }
    }
}
