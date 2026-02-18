using BCrypt.Net;
using Microsoft.AspNetCore.Http.HttpResults;
using MvcApp.Helper;
using MvcApp.Repository.Interfaces;
using MvcApp.Services.Interfaces;
using MvcApp.Models.Common;
using MvcApp.Models.Entities;
using MvcApp.Models.ViewModels;
using System.Diagnostics;

namespace MvcApp.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<UserService> _logger;

        public UserService(IUserRepository userRepository,IWebHostEnvironment webHostEnvironment,
            ILogger<UserService> logger) 
        { 
            _userRepository = userRepository;
            _env = webHostEnvironment;
            _logger = logger;
        }

        public async Task<UserProfileViewModel?> GetUserProfile(int id)
        {
            _logger.LogInformation("Fetching profile for User {UserId}", id);

            var result = await _userRepository.GetUserProfile(id);

            if (result == null)
            {
                _logger.LogWarning("UserProfile retrieval failed: User {UserId} not found.", id);

                return null;
            }

            _logger.LogWarning("UserProfile retrieval success for User {UserId}.", id);


            return new UserProfileViewModel
            {
                FirstName = result.FirstName,
                LastName = result.LastName,
                DisplayName = result.DisplayName ?? "",
                DateOfBirth = result.DateOfBirth.ToString("dd/MM/yyyy"),
                Day =    result.DateOfBirth.Day,
                Month = result.DateOfBirth.Month,
                Year = result.DateOfBirth.Year,
                Age = result.Age,
                Gender = result.Gender ? "Male" : "Female",
                Address = result.Address ?? "",
                City = result.City ?? "",
                State = result.State ?? "",
                ZipCode = result.ZipCode,
                Phone = result.Phone,
                Mobile = result.Mobile ?? "",    
                ProfilePath = result.ProfileImagePath ?? ""
            };
            
        }

        public async Task<AccountResult> UpdateUserProfile(int id, UserProfileViewModel model)
        {
            _logger.LogInformation("Updating profile for User {UserId}", id);

            var dob = new DateTime(model.Year,model.Month,model.Day);
            var Today = DateTime.Today;
            int age = Today.Year - dob.Year;

            if (dob.Month > Today.Month || (dob.Month == Today.Month && Today.Day < dob.Day))
            {
                age--;
            }
            if (age < 13)
            {
                _logger.LogWarning("Registration rejected: Underage user {id}", id);
                return new AccountResult { IsSuccess = false, ErrorMessage = "Underage!" };

            }


            var updatemodel = new ProfileUpdateViewModel
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                DisplayName = model.DisplayName,
                DateOfBirth = dob,
                Gender = model.GenderCode,
                Address = model.Address,
                City = model.City,
                State= model.State,
                ZipCode= model.ZipCode,
                Phone= model.Phone,
                Mobile = model.Mobile
            };

            var result = await _userRepository.UpdateUserProfile(id, updatemodel, age);


            if (result.ResultCode == 1)
            {
                _logger.LogInformation("Profile updated successfully for User {UserId}.", id);

                return new AccountResult { IsSuccess = true};
            }
            if (result.ResultCode == -1)
            {
                _logger.LogWarning("Update failed: User {UserId} not found.", id);

                return new AccountResult { IsSuccess = false, ErrorMessage = "User Not Found" };
            }
            _logger.LogError("Update failed for User {UserId}. Server Code: {ResultCode}", id, result.ResultCode);

            return new AccountResult { IsSuccess = false, ErrorMessage = "unhandled exception" };

        

        }

        public async Task<AccountResult> ChangePassword(int id,string oldpassword,string password)
        {
            if (oldpassword == password)
            {
                _logger.LogWarning("Password change failed: New password is same as old for User {UserId}", id);

                return new AccountResult { IsSuccess = false, ErrorMessage = "New password cannot be the same as the old password." };
            }
            var currentPassword = await _userRepository.GetPasswordById(id);


            if (currentPassword == null)
            {
                _logger.LogWarning("Password change failed: User {UserId} not found.", id);

                return new AccountResult { IsSuccess = false, ErrorMessage = "Invalid Id" };
            }
            if (!BCrypt.Net.BCrypt.Verify(oldpassword, currentPassword))
            {
                _logger.LogWarning("Password change failed: Invalid current password for User {UserId}", id);

                return new AccountResult { IsSuccess = false, ErrorMessage = "You Entered ab wrong Password" };
            }
            string hashedNewPassword = BCrypt.Net.BCrypt.HashPassword(password);

            var response  = await _userRepository.SavePassword(id, hashedNewPassword);


            if (!response)
            {
                _logger.LogError("Password change failed for User {UserId}: Database save error.", id);

                return new AccountResult { IsSuccess = false, ErrorMessage = "password changing failed" };
            }
            _logger.LogError("Password changed successfully for User {UserId}", id);

            return new AccountResult { IsSuccess = true };

        }

        public async Task<AccountResult> UpdateImage(int id, IFormFile file)
        {

            byte[] bytes;

            using(var ms = new MemoryStream())
            {
                await file.CopyToAsync(ms);
                bytes = ms.ToArray();

            }

            string folder = Path.Combine(_env.WebRootPath, "uploads","users",id.ToString());

            Directory.CreateDirectory(folder);

            var existingFiles = Directory.GetFiles(folder);

            foreach (var files in existingFiles)
            {
                File.Delete(files);
            }

            string ext = Path.GetExtension(file.FileName).ToLower();
            string fileName = "profile" + ext;

            string fullPath = Path.Combine(folder,fileName);

            using var stram = new FileStream(fullPath,FileMode.Create);
            await file.CopyToAsync(stram);

            string relativePath =$"/uploads/users/{id}/{fileName}";


            var response = await _userRepository.UploadImage(id, bytes, relativePath);

            if (!response)
            {
                _logger.LogError("Database update failed for image upload. User: {UserId}", id);

                return new AccountResult { IsSuccess = false, ErrorMessage = "Image Uploading Failed" };
            }

            _logger.LogInformation("Profile image updated successfully for User {UserId}", id);

            return new AccountResult { IsSuccess = true };


        }


    }
}
