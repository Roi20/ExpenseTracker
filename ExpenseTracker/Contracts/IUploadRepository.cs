using ExpenseTracker.Data;
using ExpenseTracker.Models;

namespace ExpenseTracker.Contracts
{
    public interface IUploadRepository 
    {

        Task UploadProfilePicture(ProfilePicture model, string userId);

        Task UpdateUser(AppIdentityUser userModel);

        Task <AppIdentityUser> GetUser(string userId);
    }
}
