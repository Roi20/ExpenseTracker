
using ExpenseTracker.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace ExpenseTracker.Services
{
    public class RoleSeederService : IHostedService
    {

        private readonly IServiceProvider _serviceProvider;

        public RoleSeederService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            //Wait for migration to finish
            await MigrationSignal.MigrationCompleted.Task;

            using var scope = _serviceProvider.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppIdentityUser>>();

            var roles = new[] { "Admin", "Moderator", "User" };
           
            foreach (var role in roles)
            { 
                if(!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            var email = "admin@admin.com";
            var password = "@AdminPassword123";

            var userEmail = await userManager.FindByEmailAsync(email);

            if(userEmail == null)
            {
                var user = new AppIdentityUser
                {
                    FirstName = "Admin",
                    LastName = "Admin",
                    Email = email,
                    UserName = email,
                    EmailConfirmed = true
                };

                await userManager.CreateAsync(user, password);
                await userManager.AddToRoleAsync(user, "Admin");

            }

           
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    }
}
