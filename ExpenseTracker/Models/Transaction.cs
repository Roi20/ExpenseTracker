using ExpenseTracker.Contracts;
using ExpenseTracker.Data;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace ExpenseTracker.Models
{
    public class Transaction : IBaseModel
    {
        [Key]
        public int TransactionId { get; set; }

        [Required(ErrorMessage = "Amount was required")]
        [Range(1, int.MaxValue, ErrorMessage = "Amount must be greater than 0")]
        public int Amount { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string? Note { get; set; }

        public DateOnly Date { get; set; } = DateOnly.FromDateTime(DateTime.Now);

        [DisplayName("Category")]
        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        [ForeignKey("User")]
        public string User_Id { get; set; }

        public virtual Category Category { get; set; }
        public virtual AppIdentityUser User {get; set;}

        public string? FormattedAmount
        {
            get
            {
                return $"{(Category == null || Category.Type == "Expense" ? '-' : '+' )} {Amount.ToString("PHP#,##0")}";
            }
        }

        public string? CategoryName
        {
            get
            {
                return  Category == null ? string.Empty : Category.Title;
            }
        }
    }
}
