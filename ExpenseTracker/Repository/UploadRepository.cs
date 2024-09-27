using ExpenseTracker.Context;
using ExpenseTracker.Contracts;
using ExpenseTracker.Data;
using ExpenseTracker.Models;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;

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



            _user.Entry(userModel).State = EntityState.Modified;
            await _context.SaveChangesAsync();
       
       
        }

        public async Task UploadProfilePicture(ProfilePicture model, string userId)
        {
            var file = model.ProfileImage;
            if(file != null && file.Length > 0)
            {
                const long maxSize = 2 * 1024 * 1024;
                if(file.Length > maxSize)
                {
                    throw new Exception("The file size must be less than 2MB.");
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
        }
    }
}
