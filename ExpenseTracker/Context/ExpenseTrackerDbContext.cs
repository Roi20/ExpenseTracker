using ExpenseTracker.Data;
using ExpenseTracker.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Context
{
    public class ExpenseTrackerDbContext : IdentityDbContext<AppIdentityUser>
    {
        public ExpenseTrackerDbContext(DbContextOptions<ExpenseTrackerDbContext> options) : base(options)
        {
            
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

    }
}
