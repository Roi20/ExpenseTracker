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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Category>()
                   .HasOne(c => c.User)
                   .WithMany(u => u.Categories)
                   .HasForeignKey(c => c.User_Id)
                   .HasConstraintName("FK_Category_AspNetUsers");

            builder.Entity<Transaction>()
                  .HasOne(c => c.User)
                  .WithMany(u => u.Transactions)
                  .HasForeignKey(c => c.User_Id)
                  .HasConstraintName("FK_Transaction_AspNetUsers");
        }


        public DbSet<Category> Categories { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

    }
}
