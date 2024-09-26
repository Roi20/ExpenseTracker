using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Models
{
    public class ProfilePicture
    {
        [Display(Name = "Upload Profile Picture")]
        public IFormFile? ProfileImage { get; set; }


    }
}
