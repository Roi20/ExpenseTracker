using ExpenseTracker.Context;
using ExpenseTracker.Contracts;
using ExpenseTracker.Data;
using ExpenseTracker.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using MimeKit.Cryptography;
using System.Collections.Concurrent;

namespace ExpenseTracker.Hubs
{
    public class NotificationHub : Hub<INotificationHub>
    {

        private readonly INotificationRepository _notificationRepo;
        private readonly UserManager<AppIdentityUser> _userManager;

        public NotificationHub(INotificationRepository notificationRepo, UserManager<AppIdentityUser> userManager)
        {
            _notificationRepo = notificationRepo;
            _userManager = userManager;
        }

        private static ConcurrentDictionary<string, bool> UserNotifications = new ConcurrentDictionary<string, bool>();


        public async Task SendNotification(string title, string message)
        {
            foreach (var connection in UserNotifications.Keys)
            {
                UserNotifications[connection] = true;
            }
            await Clients.All.ReceiveNotification(title, message, $"{DateTime.Now.ToString("g")}", false);
        }

        public async Task ClearNotification()
        {
            var userId = Context.UserIdentifier;

            if (UserNotifications.ContainsKey(userId))
            {
                UserNotifications[userId] = false;
            }
            await Clients.User(userId).NotificationCleared();

        }

        public override async Task OnConnectedAsync()
        {

            var userId = Context.UserIdentifier;

            var adminId = await _userManager.FindByIdAsync(userId);

            if (await _userManager.IsInRoleAsync(adminId, "Admin"))
                return;


            var notifications = await _notificationRepo.GetAllUserNotification(userId);

            var displayedNotifications = notifications.OrderByDescending(x => x.TimeStamp)
                                                      .Take(20)
                                                      .ToList();

            var unreadNotifications = displayedNotifications.Where(x => !x.IsRead).ToList();

            if (!unreadNotifications.Any())
            {
                await Clients.User(userId).NotificationCleared();
                return;
            }

            UserNotifications.TryAdd(userId, false);

            if(UserNotifications.TryGetValue(userId, out var hasNotifications))
            {
                await Clients.Caller.ReceiveNotificationIndicator();
            }

            await base.OnConnectedAsync();
        }


    }
}
