using ExpenseTracker.Data;
using ExpenseTracker.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

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
                   .OnDelete(DeleteBehavior.Restrict)
                   .HasConstraintName("FK_Transaction_AspNetUsers");

            builder.Entity<Transaction>()
                   .HasOne(t => t.Category)
                   .WithMany(u => u.Transactions)
                   .HasForeignKey(t => t.CategoryId)
                   .HasConstraintName("FK_Transaction_Categories");


        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<AdminNotification> AdminNotifications { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
    }
}
