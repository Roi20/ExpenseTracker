using ExpenseTracker.Context;
using ExpenseTracker.Contracts;
using ExpenseTracker.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using ExpenseTracker.Data;
using ExpenseTracker.Common;
using ExpenseTracker.Services;
//ExpenseTrackerDbContextConnection
var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;
var CONNECTION_STRING = config.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");


//Google Authentication
builder.Services.AddAuthentication().AddGoogle(googleOptions =>
{

    googleOptions.ClientId = config.GetSection("GoogleAuthentication:ClientId").Value ?? "ClientId Not Found";
    googleOptions.ClientSecret = config.GetSection("GoogleAuthentication:ClientSecret").Value?? "ClientSecret Not Found";


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
builder.Services.AddDefaultIdentity<AppIdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
       .AddRoles<IdentityRole>()
       .AddEntityFrameworkStores<ExpenseTrackerDbContext>();

// Repository Dependency
builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
builder.Services.AddScoped<ICategoryRepository,  CategoryRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<IDashboardRepository, DashboardRepository>();
builder.Services.AddScoped<IUploadRepository, UploadRepository>();

//EmailService Dependency
builder.Services.AddScoped<IEmailServiceAsync, EmailServiceAsync>();

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddRazorPages();

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
//app.UseAuthentication();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Dashboard}/{action=Index}/{id?}");

app.MapRazorPages();

//Seeding roles
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
  


app.Run();
