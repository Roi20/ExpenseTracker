using ExpenseTracker.Models;
using Microsoft.AspNetCore.Identity;

namespace ExpenseTracker.Data
{
    public class AppIdentityUser : IdentityUser
    {
        public virtual ICollection<Category> Categories { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
