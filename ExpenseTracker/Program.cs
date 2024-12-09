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
using Microsoft.AspNetCore.Authentication.Cookies;


//ExpenseTrackerDbContextConnection
var builder = WebApplication.CreateBuilder(args);

builder.Logging.SetMinimumLevel(LogLevel.Debug); 
builder.Logging.AddConsole(); 
builder.Logging.AddDebug();

builder.Configuration.AddUserSecrets<Program>();

var config = builder.Configuration;
var CONNECTION_STRING = config.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");


var conString = builder.Configuration["ConnectionStrings:DefaultConnection"];
var eUser = builder.Configuration["EmailSettings:UserName"];
var ePass = builder.Configuration["EmailSettings:Password"];
var gId = builder.Configuration["GoogleAuthentication:ClientId"];
var gSecret = builder.Configuration["GoogleAuthentication:ClientSecret"];

Console.WriteLine($"Google Id: {gId}");
Console.WriteLine($"Google Secret:{gSecret}");
Console.WriteLine($"ConnectionString: {conString}");
Console.WriteLine($"Initial ConnectionString: {CONNECTION_STRING}");
Console.WriteLine($"Email User: {eUser}");
Console.WriteLine($"Email Password: {ePass}");

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
    options.Cookie.SameSite = SameSiteMode.None;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
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


//EmailService Dependency
builder.Services.AddScoped<IEmailServiceAsync, EmailServiceAsync>();

//SignalR
builder.Services.AddSignalR();

// Add services to the container.
builder.Services.AddControllersWithViews();

//Razor Pages 
builder.Services.AddRazorPages();


//Notification Service Dependency
builder.Services.AddScoped<NotificationService>();


//Startup Migration Service
builder.Services.AddHostedService<StartUpMigrationService>();


//Add Hangfire server
builder.Services.AddHangfireServer();
//Hangfire
builder.Services.AddHangfire(options => options.UseSqlServerStorage(CONNECTION_STRING));
builder.Services.AddHangfireServer();

//Role Seeding Service
builder.Services.AddHostedService<RoleSeederService>();

//Hangfire Service
builder.Services.AddHostedService<HangfireSchedulerService>();

var app = builder.Build();


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


app.UseMiddleware<ActivityMiddlerware>();

app.UseHangfireDashboard();



app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapHub<NotificationHub>("/notificationHub");



app.Run();
