using ExpenseTracker.Context;
using ExpenseTracker.Contracts;
using ExpenseTracker.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using ExpenseTracker.Data;

var builder = WebApplication.CreateBuilder(args);



var config = builder.Configuration;
var CONNECTION_STRING = builder.Configuration.GetConnectionString("ExpenseTrackerAppDbConn") ?? throw new InvalidOperationException("ConnectionString 'ExpenseTrackerAppDbConn' not found.");


// Sql Dependency
builder.Services.AddDbContext<ExpenseTrackerDbContext>(options =>
        options.UseSqlServer(CONNECTION_STRING));

//builder.Services.AddDatabaseDeveloperPageExceptionFilter();

//Identity
builder.Services.AddDefaultIdentity<AppIdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
       .AddEntityFrameworkStores<ExpenseTrackerDbContext>();

// Repository Dependency
builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
builder.Services.AddScoped<ICategoryRepository,  CategoryRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<IDashboardRepository, DashboardRepository>();

// Add services to the container.
builder.Services.AddControllersWithViews();

//Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NCaF5cXmZCdkx3THxbf1x0ZFxMYl5bQHRPMyBoS35RckVkW39fcHRRQ2BdWUR1");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

   // app.UseMigrationsEndPoint();

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

app.Run();
