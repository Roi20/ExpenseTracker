using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Newtonsoft.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Models
{
    public class ProfilePicture
    {
        [Display(Name = "Upload Profile Picture")]
        public IFormFile? ProfileImage { get; set; }


    }
}
