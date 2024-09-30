using ExpenseTracker.Data;
using ExpenseTracker.Models;

namespace ExpenseTracker.Common
{
    public class UserViewModel
    {
        public ProfilePicture ProfilePicture { get; set; }
        public AppIdentityUser User { get; set; }

    }
}
