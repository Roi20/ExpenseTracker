using ExpenseTracker.Data;
using Microsoft.AspNetCore.Identity;
using System.Xml.Schema;

namespace ExpenseTracker.Common
{
    public class ActivityMiddlerware
    {

        private readonly RequestDelegate _next;
        private readonly IServiceScopeFactory _scopeFactory;

        public ActivityMiddlerware(RequestDelegate next, IServiceScopeFactory scopeFactory)
        {
            _next = next;
            _scopeFactory = scopeFactory;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.User.Identity.IsAuthenticated) 
            {
                
                using var scope = _scopeFactory.CreateScope();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppIdentityUser>>();
                var user = await userManager.FindByNameAsync(context.User.Identity.Name);
                if(user != null)
                {

                    user.LastActivityDate = DateTime.UtcNow;
                    await userManager.UpdateAsync(user);
                   
                }
            }

            await _next(context);
        }

    }
}
