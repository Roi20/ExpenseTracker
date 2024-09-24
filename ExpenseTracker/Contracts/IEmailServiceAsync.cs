namespace ExpenseTracker.Contracts
{
    public interface IEmailServiceAsync
    {

        Task EmailSendAsync(string email, string subject, string body);

    }
}
