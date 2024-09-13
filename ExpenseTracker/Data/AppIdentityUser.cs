using ExpenseTracker.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Security.Policy;

namespace ExpenseTracker.Data
{
    public class AppIdentityUser : IdentityUser
    {

        [Required(ErrorMessage = "First Name is Required")]
        [Display(Name = "First Name")]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is Required")]
        [Display(Name = "Last Name")]
        [StringLength(100)]
        public string LastName { get; set; }

        [StringLength(100)]
        public string? Occupation { get; set; }

        [StringLength(100)]
        public string? Business { get; set; }

        [StringLength(100), Display(Name = "Other Source Of Income")]
        public string? SourceOfIncome { get; set; }

        public virtual ICollection<Category> Categories { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }

    }
}
