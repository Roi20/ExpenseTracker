namespace ExpenseTracker.Contracts
{
    public interface INotificationRepository
    {
        Task SendNotificationAsync(string title, string message);
        Task UpdateNotificationAsync(int notificationId, string newTitle, string newMessage);
    }
}
