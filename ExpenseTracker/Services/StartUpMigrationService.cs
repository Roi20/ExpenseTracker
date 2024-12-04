
using ExpenseTracker.Context;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Services
{
    public class StartUpMigrationService : IHostedService
    {

        private readonly IServiceProvider _serviceProvider;

        public StartUpMigrationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();

            try
            {
                var context = scope.ServiceProvider.GetRequiredService<ExpenseTrackerDbContext>();
                await context.Database.MigrateAsync(cancellationToken);

                MigrationSignal.MigrationCompleted.SetResult(true);
               
         
            }
            catch(Exception ex)
            {
                var logger = _serviceProvider.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred while migrating the database.");
            }

        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    }

    public static class MigrationSignal
    {
        public static TaskCompletionSource<bool> MigrationCompleted { get; } = new TaskCompletionSource<bool>(); 
    }

}
