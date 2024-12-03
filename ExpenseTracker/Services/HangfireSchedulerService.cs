
namespace ExpenseTracker.Services
{
    public class HangfireSchedulerService : IHostedService
    {

        private readonly IServiceProvider _serviceProvider;

        public HangfireSchedulerService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {

            using var scope = _serviceProvider.CreateScope();
            var notificationService = scope.ServiceProvider.GetRequiredService<NotificationService>();
            notificationService.ScheduleRecurringJob();
            await Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    }
}
