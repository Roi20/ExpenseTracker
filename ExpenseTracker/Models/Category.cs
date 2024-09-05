using ExpenseTracker.Contracts;
using ExpenseTracker.Data;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseTracker.Models
{
    public class Category : IBaseModel
    {
        [Key]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Title field was required")]
        [StringLength(50)]
        [Column(TypeName = "nvarchar(50)")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Icon field was required")]
        [StringLength(5)]
        [Column(TypeName = "nvarchar(5)")]
        public string Icon { get; set; }

        [Required(ErrorMessage = "Please select between Expense and Income")]
        [StringLength(20)]
        [Column(TypeName = "nvarchar(20)")]
        public string Type { get; set; }

        [ForeignKey("AppIdentityUser")]
        public string User_Id { get; set; }

        public virtual AppIdentityUser User { get; set; }

    }
}
