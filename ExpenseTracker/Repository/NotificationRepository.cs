using ExpenseTracker.Context;
using ExpenseTracker.Contracts;
using ExpenseTracker.Data;
using ExpenseTracker.Hubs;
using ExpenseTracker.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Repository
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly DbContext _db;
        private readonly DbSet<Notification> _notification;
        private readonly IHubContext<NotificationHub, INotificationHub> _hub;

        public NotificationRepository(ExpenseTrackerDbContext db, IHubContext<NotificationHub, INotificationHub> hub)
        {
            _db = db;
            _notification = _db.Set<Notification>();
            _hub = hub;
        }
        public async Task SendNotificationAsync(string title, string message)
        {

            try
            {
                var users = await _db.Set<AppIdentityUser>().ToListAsync();

                foreach(var user in users)
                {
                    //skip users with an email that's not confirmed or inactive for more than 30days.
                    if (!user.EmailConfirmed || user.LastActivityDate < DateTime.UtcNow.AddDays(-30))
                        continue;

                    var notification = new Notification
                    {
                        UserId = user.Id,
                        Title = title,
                        Message = message,
                        IsRead = false,
                        TimeStamp = DateTime.Now,
                    };

                    _notification.Add(notification);
                }

                await _db.SaveChangesAsync();

                await _hub.Clients.All.ReceiveNotification(title, message, $"{DateTime.Now:f}", false);
                
            }
            catch (DbUpdateException)
            {
                throw;
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

        public async Task UpdateNotificationAsync(int notificationId, string newTitle, string newMessage)
        {
            try
            {
                var notifications = await _notification.Where(x => x.Id == notificationId).ToListAsync();

                foreach (var notification in notifications)
                {
                    notification.Title = newTitle;
                    notification.Message = newMessage;
                }

                await _db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw;
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
