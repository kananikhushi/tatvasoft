using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using Test.Entities;
using Test.Repo;

namespace Test.Services
{
    public class UserService
    {
        private readonly AppDbContext _context; //Access DB using ependency injection so external object can be used
        private readonly IConfiguration _config;
        public UserService(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public MessageRes Register(User user)
        {
            // 🚫 Check if user already exists by email
            if (_context.Users.Any(u => u.EmailAddress == user.EmailAddress))
            {
                return new MessageRes
                {
                    Success = false,
                    Message = "Try Different Email !!"
                };
            }

            // ✅ Set default values if not provided
            if (string.IsNullOrEmpty(user.UserType))
                user.UserType = "user";

            if (string.IsNullOrEmpty(user.UserImage))
                user.UserImage = "None"; // or set to "" if no image is used

            user.ModificationDate = DateTime.UtcNow;
            user.IsDeleted = false;

            // ✅ Save to database
            _context.Users.Add(user);
            _context.SaveChanges();

            return new MessageRes
            {
                Success = true,
                Message = "Register Successful"
            };
        }


        public MessageRes Login(string email, string password, IConfiguration config)
        {
            var user = _context.Users.FirstOrDefault(u => u.EmailAddress == email);
            if (user == null)
            {
                return new MessageRes
                {
                    Success = false,
                    Message = "Email not found"
                };
            }

            if (user.Password != password)
            {
                return new MessageRes
                {
                    Success = false,
                    Message = "Incorrect password"
                };
            }
            var token = GenerateJwtToken(user, config);

            return new MessageRes
            {
                Success = true,
                Message = $"Welcome back, {user.FirstName}!",
                Data = new
                {
                    id = user.Id,
                    FirstName = user.FirstName,
                    Token = token
                }
            };
        }
        public User? GetUserById(int id)
        {
            return _context.Users.FirstOrDefault(u => u.Id == id);
        }

        public string GenerateJwtToken(User user, IConfiguration config)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Email, user.EmailAddress),
                new Claim(ClaimTypes.Role, user.UserType)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: config["Jwt:Issuer"],
                audience: config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public readonly AppDbContext _appDbContext;

        public async Task<User> UpdateUser(UserDetails model, string webRootPath)
        {
            // Check if email exists for another user
            var isExist = _context.Users
                .Where(x => x.EmailAddress == model.EmailAddress && x.Id != model.Id && !x.IsDeleted)
                .FirstOrDefault();

            if (isExist != null)
                throw new Exception("Email already exists");

            var existingUser = _context.Users
                .Where(x => x.Id == model.Id && !x.IsDeleted)
                .FirstOrDefault() ?? throw new Exception("User does not exist");

            // Handle image update
            if (model.ProfileImage != null)
            {
                // Delete old image if exists
                if (!string.IsNullOrEmpty(existingUser.UserImage))
                {
                    string oldImageFullPath = Path.Combine(webRootPath, existingUser.UserImage.Replace("/", Path.DirectorySeparatorChar.ToString()));
                    if (File.Exists(oldImageFullPath))
                    {
                        File.Delete(oldImageFullPath);
                    }
                }

                // Save new image
                string imagePath = await SaveImageAsync(model.ProfileImage, "Images", webRootPath);
                existingUser.UserImage = imagePath;
            }

            // Update user details
            existingUser.FirstName = model.FirstName;
            existingUser.LastName = model.LastName;
            existingUser.PhoneNumber = model.PhoneNumber;
            existingUser.EmailAddress = model.EmailAddress;
            existingUser.UserType = model.UserType;
            existingUser.ModificationDate = DateTime.UtcNow;

            _context.Users.Update(existingUser);
            await _context.SaveChangesAsync();

            return existingUser;
        }

        private async Task<string> SaveImageAsync(IFormFile file, string folderName, string webRootPath)
        {
            if (file == null || file.Length == 0)
                return null;

            string uploadsFolder = Path.Combine(webRootPath, folderName);
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            string fileExtension = Path.GetExtension(file.FileName);
            string fileName = $"{Path.GetFileNameWithoutExtension(file.FileName)}_{DateTime.UtcNow:yyyyMMddHHmmss}{fileExtension}";
            string fullPath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Return relative path for DB storage
            return Path.Combine(folderName, fileName).Replace("\\", "/");
        }
    }

}