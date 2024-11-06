namespace ExpenseTracker.Contracts
{
    public interface INotificationHub
    {
        Task ReceiveNotification(string title, string message, string timeStamp, bool isRead);
    }
}
