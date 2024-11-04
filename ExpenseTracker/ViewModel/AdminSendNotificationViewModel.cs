using ExpenseTracker.Models;

namespace ExpenseTracker.ViewModel
{
    public class AdminSendNotificationViewModel
    {
        public IEnumerable<Notification> Notifications { get; set; }
        public Notification Notification { get; set; }

    }
}
