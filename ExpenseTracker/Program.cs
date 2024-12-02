using ExpenseTracker.Context;
using ExpenseTracker.Contracts;
using ExpenseTracker.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using ExpenseTracker.Data;
using ExpenseTracker.Common;
using ExpenseTracker.Services;
using ExpenseTracker.Hubs;
using Hangfire;

//ExpenseTrackerDbContextConnection
var builder = WebApplication.CreateBuilder(args);

builder.Logging.SetMinimumLevel(LogLevel.Debug); 
builder.Logging.AddConsole(); 
builder.Logging.AddDebug();

builder.Configuration.AddUserSecrets<Program>();

var config = builder.Configuration;
var CONNECTION_STRING = config.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");


var conString = builder.Configuration["ConnectionStrings:DefaultConnection"];
Console.WriteLine($"ConnectionString: {conString}");


//Add Environment Variables
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                     .AddEnvironmentVariables()
                     .AddUserSecrets<Program>();


//Google Authentication
builder.Services.AddAuthentication().AddGoogle(googleOptions =>
{

    googleOptions.ClientId = builder.Configuration["GoogleAuthentication:ClientId"] ?? "ClientId Not Found";
    googleOptions.ClientSecret = builder.Configuration["GoogleAuthentication:ClientSecret"] ?? "ClientSecret Not Found";


});

//EmailSettings - Email Sender Service
builder.Services.Configure<EmailSettings>(config.GetSection("EmailSettings"));


// Sql Dependency
builder.Services.AddDbContext<ExpenseTrackerDbContext>(options =>
        options.UseSqlServer(CONNECTION_STRING, 
        sqlOptions => sqlOptions.EnableRetryOnFailure(
            
                  maxRetryCount: 5,
                  maxRetryDelay: TimeSpan.FromSeconds(30),
                  errorNumbersToAdd: null
            )));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

//Identity
builder.Services.AddDefaultIdentity<AppIdentityUser>(options =>
         {
             options.SignIn.RequireConfirmedAccount = true;
             options.Password.RequiredLength = 8;
             options.Password.RequireDigit = true;
             options.Password.RequiredUniqueChars = 1;
             options.Password.RequireLowercase = true;
             options.Password.RequireUppercase = true;
             options.Password.RequireNonAlphanumeric = false;

         })
       .AddRoles<IdentityRole>()
       .AddEntityFrameworkStores<ExpenseTrackerDbContext>();

//Cookie
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromDays(90);
    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    options.SlidingExpiration = true;

});



// Repository Dependency
builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
builder.Services.AddScoped<ICategoryRepository,  CategoryRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<IDashboardRepository, DashboardRepository>();
builder.Services.AddScoped<IUploadRepository, UploadRepository>();
builder.Services.AddScoped<IAdminUserRepository, AdminUserRepository>();
builder.Services.AddScoped<IAdminDashboardRepository, AdminDashboardRepository>();
builder.Services.AddScoped<IAdminManageRoleRepository, AdminManageRoleRepository>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<IAuditLogRepository, AuditLogRepository>();

//Notification Service Dependency
//builder.Services.AddScoped<NotificationService>();

//EmailService Dependency
builder.Services.AddTransient<IEmailServiceAsync, EmailServiceAsync>();

//SignalR
builder.Services.AddSignalR();

// Add services to the container.
builder.Services.AddControllersWithViews();

//Razor Pages 
builder.Services.AddRazorPages();


//Add Hangfire server
//builder.Services.AddHangfireServer();
//Hangfire
//builder.Services.AddHangfire(options => options.UseSqlServerStorage(CONNECTION_STRING));
//builder.Services.AddHangfireServer();



var app = builder.Build();

//app.Lifetime.ApplicationStopping.Register(() =>
//{
  //  var server = app.Services.GetService<BackgroundJobServer>();
   // server?.Dispose();
//});


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

    app.UseMigrationsEndPoint();

}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();


app.UseAuthorization();

app.MapRazorPages();

/*
//Apply Migration at Startup
using var migrationScope = app.Services.CreateScope();

var services = migrationScope.ServiceProvider;

try
{
    var context = services.GetRequiredService<ExpenseTrackerDbContext>();
    context.Database.Migrate();
}
catch (Exception ex)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occurred while migrating the database.");
}
*/

/*/Seeding roles
using var scope = app.Services.CreateScope();
var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

var roles = new[] {"Admin", "Moderator", "User"};

foreach(var role in roles)
{

    if (!await roleManager.RoleExistsAsync(role))
        await roleManager.CreateAsync(new IdentityRole(role));

}


//Adding Admin Role
using var adminScope = app.Services.CreateScope();

var userManager = adminScope.ServiceProvider.GetRequiredService<UserManager<AppIdentityUser>>();

var email = "admin123@admin.com";
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
*/

app.UseMiddleware<ActivityMiddlerware>();



app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapHub<NotificationHub>("/notificationHub");


//app.UseHangfireDashboard();


//Hangfire ScheduleRecurringJob NotificationService
//using var scheduleScope = app.Services.CreateScope();
//var notificationService = scheduleScope.ServiceProvider.GetRequiredService<NotificationService>();
//notificationService.ScheduleRecurringJob();


app.Run();
