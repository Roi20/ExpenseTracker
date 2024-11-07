using ExpenseTracker.Models;

namespace ExpenseTracker.Contracts
{
    public interface INotificationRepository
    {
        Task SendNotificationAsync(string title, string message);
        Task UpdateNotificationAsync(int adminNotificationId, string newTitle, string newMessage);
        Task DeleteNotificationAsync(int adminNotificationId);
        Task<AdminNotification> GetAdminNotificationId(int id);
        Task<IEnumerable<AdminNotification>> GetAllAdminNotifications();
    }
}
