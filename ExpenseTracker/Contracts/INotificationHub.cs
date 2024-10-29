namespace ExpenseTracker.Contracts
{
    public interface INotificationHub
    {
        Task ReceiveNotification(string title, string message, DateTime timeStamp);
    }
}
