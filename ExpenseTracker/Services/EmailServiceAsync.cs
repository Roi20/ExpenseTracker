using ExpenseTracker.Common;
using ExpenseTracker.Contracts;
using Microsoft.Extensions.Options;
using MimeKit;
//using System.Net.Mail;
using MailKit.Net.Smtp;
using MailKit.Security;

namespace ExpenseTracker.Services
{
    public class EmailServiceAsync : IEmailServiceAsync
    {

        private readonly EmailSettings _emailSettings;

        public EmailServiceAsync(IOptions<EmailSettings> option)
        {
            _emailSettings = option.Value;
        }


        public async Task EmailSendAsync(string email, string subject, string body)
        {
            var message = new MimeMessage();

            message.From.Add(MailboxAddress.Parse(_emailSettings.UserName));
            message.To.Add(MailboxAddress.Parse(email));
            message.Subject = subject;
            message.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = body };

            using var sender = new SmtpClient();
            sender.Connect(_emailSettings.Server, _emailSettings.Port, SecureSocketOptions.StartTls);
            sender.Authenticate(_emailSettings.UserName, _emailSettings.Password);

            await sender.SendAsync(message);

        }
    }
}
