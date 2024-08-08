using ExpenseTracker.Context;
using ExpenseTracker.Contracts;
using ExpenseTracker.Repository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var config = builder.Configuration;

// Sql Dependency
builder.Services.AddDbContext<ExpenseTrackerDbContext>(options =>
options.UseSqlServer(config.GetConnectionString("ExpenseTrackerDbConn")));

// Repository Dependency
builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
builder.Services.AddScoped<ICategoryRepository,  CategoryRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();

Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NCaF5cXmZCdkx3THxbf1x0ZFxMYl5bQHRPMyBoS35RckVkW39fcHRRQ2BdWUR1");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Transaction}/{action=Update}/{id?}");

app.Run();
