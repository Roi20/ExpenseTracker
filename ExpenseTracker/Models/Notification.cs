using ExpenseTracker.Data;
using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseTracker.Models
{
    public class Notification
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public DateTime TimeStamp { get; set; }
        public bool IsRead { get; set; } = false;

        [ForeignKey("AdminNotification")]
        public int AdminNotificationId { get; set; }
        public virtual AdminNotification AdminNotification {get; set;}




    }
}
