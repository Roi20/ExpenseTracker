using ExpenseTracker.Contracts;
using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Models
{
    public class AuditLog : IBaseModel
    {
        [Key]
        public int LogId { get; set; }
        public string User_Id { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; } 
        public string Action { get; set; } 
        public string TimeStamp { get; set; } 
        public string EntityId { get; set; } 
        public string EntityType { get; set; } 
        public string Details { get; set; }
    }
}
