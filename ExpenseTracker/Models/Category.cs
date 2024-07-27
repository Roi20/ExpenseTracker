using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseTracker.Models
{
    public class Category
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
        public string Type { get; set; } = "Unspecified";


        public string CategoryWithIcon 
        {
            get
            {
                return Icon + "  " + "  " +Title;
            }

        }


    }
}
