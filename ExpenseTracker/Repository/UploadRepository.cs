using ExpenseTracker.Context;
using ExpenseTracker.Contracts;
using ExpenseTracker.Data;
using ExpenseTracker.Models;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using Syncfusion.EJ2.Spreadsheet;

namespace ExpenseTracker.Repository
{
    public class UploadRepository : IUploadRepository
    {

        private readonly IWebHostEnvironment _environment;
        private readonly DbContext _context;
        private readonly DbSet<AppIdentityUser> _user;

        public UploadRepository(IWebHostEnvironment environment, ExpenseTrackerDbContext context)
        {
            _environment = environment;
            _context = context;
            _user = _context.Set<AppIdentityUser>();
            
        }

        public async Task<AppIdentityUser> GetUser(string userId)
        {
            return await _user.FirstOrDefaultAsync(x => x.Id == userId);
        }

        
        public async Task UpdateUser(AppIdentityUser userModel)
        {
            try
            {
                _user.Entry(userModel).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw new Exception("Unable to update Database");
            }
            catch (Exception)
            {
                throw new Exception("An error occured while trying to update the user profile.");
            }
        }

        public async Task UploadProfilePicture(ProfilePicture model, string userId)
        {

            
            var file = model.ProfileImage;
            if(file != null && file.Length > 0)
            {
                const long maxSize = 2 * 1024 * 1024;
                if(file.Length > maxSize)
                {
                    throw new ArgumentException("The file size should not exceed 2MB.");
                }

                var allowedExtension = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var fileExtension = Path.GetExtension(file.FileName);
                if (!allowedExtension.Contains(fileExtension))
                {
                    throw new ArgumentException("Invalid file extension, Only image are allowed");
                }

                var uniqueFileName = $"{Path.GetFileNameWithoutExtension(file.FileName)}_{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                var filePath = Path.Combine(_environment.WebRootPath, "uploads", uniqueFileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                await file.CopyToAsync(stream);

                var user = await GetUser(userId);
                var relativePath = Path.Combine("uploads", uniqueFileName);
                user.ProfilePicturePath = relativePath;
                await UpdateUser(user);
                
            }
            else
            {
                throw new Exception("Error occur, Can't update profile");
            }
        }
    }
}
