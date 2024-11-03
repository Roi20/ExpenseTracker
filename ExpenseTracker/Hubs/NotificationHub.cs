using ExpenseTracker.Context;
using ExpenseTracker.Contracts;
using ExpenseTracker.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Hubs
{
    public class NotificationHub : Hub<INotificationHub>
    {
        private readonly DbContext _db;
        private readonly DbSet<Notification> _notification;

        public NotificationHub(ExpenseTrackerDbContext db)
        {
            _db = db;
            _notification = _db.Set<Notification>();
        }

        public async Task SendNotification(string userId, string title, string message, DateTime timeStamp)
        {
            try
            {

                var notification = new Notification
                {
                    UserId = userId,
                    Title = title,
                    Message = message,
                    IsRead = false,
                    TimeStamp = timeStamp
                };

                _notification.Add(notification);
                await _db.SaveChangesAsync();

                await Clients.User(userId).ReceiveNotification(title, message, timeStamp, false);


            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }


    }
}
