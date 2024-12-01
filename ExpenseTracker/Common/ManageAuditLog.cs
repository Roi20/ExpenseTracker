using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Security.Policy;

namespace ExpenseTracker.Common
{
    public class ManageAuditLog
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
        public string Action { get; set; }
        public string Details { get; set; }

        [DisplayName("Time")]
        public string TimeStamp { get; set; }
    }
}
