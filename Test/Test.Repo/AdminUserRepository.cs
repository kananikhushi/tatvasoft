using Microsoft.AspNetCore.Http;

using System;
using System.Collections.Generic;
using System.Linq;
using Test.Entities;

namespace Test.Repo
{
    public class AdminUserRepository(AppDbContext _appDbContext)
    {
        public List<UserDetails> UserDetailsList()
        {
            var res = _appDbContext.Users.Where(x => x.UserType == "user" && !x.IsDeleted)
                .Select(x => new UserDetails
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    PhoneNumber = x.PhoneNumber,
                    EmailAddress = x.EmailAddress,
                    UserType = x.UserType,
                });
            return res.ToList();
        }

        public string DeleteUser(int id)
        {
            var user = _appDbContext.Users.FirstOrDefault(x => x.Id == id);
            if (user == null) throw new Exception("Account not Exist");
            user.IsDeleted = true;
            user.ModificationDate = DateTime.UtcNow;
            _appDbContext.Users.Update(user);
            _appDbContext.SaveChanges();
            return "Account Deleted";
        }

        // ✅ Add this Register method
        public string Register(User user)
        {
            _appDbContext.Users.Add(user);
            _appDbContext.SaveChanges();
            return "User registered successfully";
        }
      

        private string? SaveImageAsync(IFormFile file, string folderName, string webRootPath)
        {
            if (file == null || file.Length == 0) return null;

            string uploadsFolder = Path.Combine(webRootPath, folderName);
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            string fileExtension = Path.GetExtension(file.FileName);
            string fileName = $"{Path.GetFileNameWithoutExtension(file.FileName)}_{DateTime.UtcNow:yyyyMMddHHmmss}{fileExtension}";
            string fullPath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            return Path.Combine(folderName, fileName).Replace("\\", "/");
        }

    }
}
