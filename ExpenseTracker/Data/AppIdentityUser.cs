using ExpenseTracker.Contracts;
using ExpenseTracker.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Policy;

namespace ExpenseTracker.Data
{
    public class AppIdentityUser : IdentityUser, IBaseModel
    {
        [NotMapped]
        public string User_Id { get; set; }

        [Required(ErrorMessage = "Email is Required.")]
        public override string? Email { get => base.Email; set => base.Email = value; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
        public string Password { get; set; }

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

        [StringLength(100), Display(Name = "Income/Occupation")]
        public string? SourceOfIncome { get; set; }

        [MaxLength]
        public string? ProfilePicturePath { get; set; }

        public virtual ICollection<Category> Categories { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
       
    }
}
