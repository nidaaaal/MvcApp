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

        public UserService(IUserRepository userRepository,IWebHostEnvironment webHostEnvironment) 
        { 
            _userRepository = userRepository;
            _env = webHostEnvironment;
        }

        public async Task<AccountResult> RegisterUser(RegisterViewModel dto)
        {
            var validateUsername = InputIdentifier.Identify(dto.UserName);

            if (validateUsername == InputIdentifier.InputType.Invalid)
                return new AccountResult { IsSuccess = false,  ErrorMessage = "Invalid Email or Phone Number format" };

            var passwordCheck = PasswordValidator.Validate(dto.Password);

            if (!passwordCheck.IsValid)
                return new AccountResult { IsSuccess = false,  ErrorMessage = passwordCheck.ErrorMessage };



            string hashedPass =  BCrypt.Net.BCrypt.HashPassword(dto.Password);

            var dob = new DateTime(dto.Year, dto.Month, dto.Day);

            var Today = DateTime.Today;


            int age = Today.Year - dto.Year;

            if(dto.Month > Today.Month || (dto.Month == Today.Month && Today.Day < dto.Day))
            {
                age--;
            }

            if(age <13)
            {
                return new AccountResult { IsSuccess = false, ErrorMessage = "Underage!" };

            }

            try
            {
                int result = await _userRepository.RegisterUser(dto, hashedPass, age,dob);

                if (result == -1)
                {
                    return new AccountResult { IsSuccess = false, ErrorMessage = "Username already exists with the username" };

                }
                if (result == 1)
                {
                    return new AccountResult { IsSuccess = true };
                }
                else
                {
                    return new AccountResult { IsSuccess = false, ErrorMessage = "Registered Failed" };

                }
            }
            catch (Exception ex)
            {
                return new AccountResult { IsSuccess = false, ErrorMessage = ex.Message };

            }
        }

        public async Task<AccountResult> LoginUser(string username, string password)
        {
            try
            {
                var result = await _userRepository.GetUserByUsername(username);

                if (result == null) return new AccountResult { IsSuccess = false, ErrorMessage = "No User Found on the corresponding username" };

                if (!BCrypt.Net.BCrypt.Verify(password, result.HashedPassword)) return new AccountResult { IsSuccess = false, ErrorMessage = "Invalid Credentials!" };


                await _userRepository.SaveLogin(result.Id);

                return new AccountResult { IsSuccess = true ,UserId = result.Id };
            }
            catch(Exception ex)
            {
                return new AccountResult { IsSuccess = false, ErrorMessage = ex.Message };

            }
        }
        public async Task<UserProfileViewModel?> GetUserProfile(int id)
        {

                var result = await _userRepository.GetUserProfile(id);

                if( result == null) return  null ;

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
            try
            {
                var dob = new DateTime(model.Year,model.Month,model.Day);
                var Today = DateTime.Today;
                int age = Today.Year - dob.Year;

                if (dob.Month > Today.Month || (dob.Month == Today.Month && Today.Day < dob.Day))
                {
                    age--;
                }
                if (age < 13)
                {
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
                    return new AccountResult { IsSuccess = true};
                }
                if (result.ResultCode == -1)
                {
                    return new AccountResult { IsSuccess = false, ErrorMessage = "User Not Found" };
                }
                return new AccountResult { IsSuccess = false, ErrorMessage = "unhandled exception" };

            }
            catch (Exception ex)
            {
                return new AccountResult { IsSuccess = false, ErrorMessage = ex.Message };

            }

        }

        public async Task<AccountResult> ChangePassword(int id,string oldpassword,string password)
        {
           var currentPassword = await _userRepository.GetPasswordById(id);


            if (currentPassword == null) return new AccountResult { IsSuccess = false, ErrorMessage = "Invalid Id" };

            if (!BCrypt.Net.BCrypt.Verify(oldpassword, currentPassword)) return new AccountResult { IsSuccess = false , ErrorMessage ="You Entered ab wrong Password"};

            string hashedNewPassword = BCrypt.Net.BCrypt.HashPassword(password);

            var response  = await _userRepository.SavePassword(id, hashedNewPassword);


            if (!response) return new AccountResult { IsSuccess = false, ErrorMessage = "password changing failed" };

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

            if(!Directory.Exists(folder))Directory.CreateDirectory(folder);

            string ext = Path.GetExtension(file.FileName).ToLower();
            string fileName = "profile" + ext;

            string fullPath = Path.Combine(folder,fileName);

            using var stram = new FileStream(fullPath,FileMode.Create);
            await file.CopyToAsync(stram);

            string relativePath =$"/uploads/users/{id}/{fileName}";


            var response = await _userRepository.UploadImage(id, bytes, relativePath);

            if (!response) return new AccountResult { IsSuccess = false,ErrorMessage = "Image Uploading Failed" };

            return new AccountResult { IsSuccess = true };


        }


    }
}
