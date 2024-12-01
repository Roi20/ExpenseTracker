using ExpenseTracker.Contracts;
using Hangfire;

namespace ExpenseTracker.Services
{
    public class NotificationService
    {
        private readonly IRecurringJobManager _recurringJobs;
        private readonly INotificationRepository _repo;

        public NotificationService(INotificationRepository repo, IRecurringJobManager recurringJobs)
        {
            _repo = repo;
            _recurringJobs = recurringJobs;
        }

        public void ScheduleRecurringJob()
        {
            _recurringJobs.AddOrUpdate(
                "Delete-Old-Notification",
                () => DeleteNotificationOlderThanAsync(30),
                Cron.Daily
                );
        }
        public async Task DeleteNotificationOlderThanAsync(int oldDays)
        {
            var cutOffDate = DateTime.Now.AddDays(-oldDays);
            await _repo.DeleteNotificationOlderThanAsync(cutOffDate);
        }
    }
}
