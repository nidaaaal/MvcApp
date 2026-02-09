using BCrypt.Net;
using Microsoft.AspNetCore.Http.HttpResults;
using MvcApp.Helper;
using MvcApp.Repository.Interfaces;
using MvcApp.Services.Interfaces;
using MvcApp.Models.Common;
using MvcApp.Models.Entities;
using MvcApp.Models.ViewModels;

namespace MvcApp.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository) 
        { 
            _userRepository = userRepository;
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

                return new AccountResult { IsSuccess = true };
            }
            catch(Exception ex)
            {
                return new AccountResult { IsSuccess = false, ErrorMessage = ex.Message };

            }
        }
        public async Task<Users?> GetUserProfile(int id)
        {
                var result = await _userRepository.GetUserProfile(id);
                return result == null ? null : result;
            
        }

        public async Task<AccountResult> UpdateUserProfile(int id, UpdateViewModel updateProfileDto)
        {
            try
            {
                var Today = DateTime.Today;
                int age = Today.Year - updateProfileDto.DateOfBirth.Year;

                if (updateProfileDto.DateOfBirth.Month > Today.Month || (updateProfileDto.DateOfBirth.Month == Today.Month && Today.Day < updateProfileDto.DateOfBirth.Day))
                {
                    age--;
                }

                var result = await _userRepository.UpdateUserProfile(id, updateProfileDto, age);

                if (result.ResultCode == 1)
                {
                    return new AccountResult { IsSuccess = false, ErrorMessage = "Profile Updated Sucessfully" };
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

    }
}
