using MvcApp.Helper;
using MvcApp.Models.Common;
using MvcApp.Models.Entities;
using MvcApp.Models.ViewModels;
using MvcApp.Repository.Interfaces;
using MvcApp.Services.Interfaces;

namespace MvcApp.Services
{
    public class AccountServices : IAccountServices
    {
        private readonly IUserRepository _userRepository;
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<UserService> _logger;
        private readonly IJwtHelper _jwtHelper;

        public AccountServices(IUserRepository userRepository, IWebHostEnvironment webHostEnvironment,
            ILogger<UserService> logger, IJwtHelper jwtHelper)
        {
            _userRepository = userRepository;
            _env = webHostEnvironment;
            _logger = logger;
            _jwtHelper = jwtHelper;
        }

        public async Task<AccountResult> RegisterUser(RegisterViewModel dto)
        {
            var validateUsername = InputIdentifier.Identify(dto.UserName);

            if (validateUsername == InputIdentifier.InputType.Invalid)
            {
                _logger.LogWarning("Registration Failed : Invalid Username Format for {UserName}", dto.UserName);

                return new AccountResult { IsSuccess = false, ErrorMessage = "Invalid Email or Phone Number format" };
            }
            var passwordCheck = PasswordValidator.Validate(dto.Password);

            if (!passwordCheck.IsValid)
            {
                _logger.LogWarning("Registration Failed : Weak Password for {UserName}", dto.UserName);

                return new AccountResult { IsSuccess = false, ErrorMessage = passwordCheck.ErrorMessage };
            }



            string hashedPass = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            var dob = new DateTime(dto.Year, dto.Month, dto.Day);

            var Today = DateTime.Today;


            int age = Today.Year - dto.Year;

            if (dto.Month > Today.Month || (dto.Month == Today.Month && Today.Day < dto.Day))
            {
                age--;
            }

            if (age < 13)
            {
                _logger.LogWarning("Registration rejected: Underage user {UserName}", dto.UserName);
                return new AccountResult { IsSuccess = false, ErrorMessage = "Underage!" };

            }

            int result = await _userRepository.RegisterUser(dto, hashedPass, age, dob);

            if (result == -1)
            {
                _logger.LogInformation("Registration rejected : Registration attempt with existing username {UserName}", dto.UserName);

                return new AccountResult { IsSuccess = false, ErrorMessage = "Username already exists with the username" };

            }
            if (result == 1)
            {
                _logger.LogInformation("User {UserName} registered successfully", dto.UserName);

                return new AccountResult { IsSuccess = true };
            }

            _logger.LogError("Unexpected registration result for {UserName}", dto.UserName);

            return new AccountResult { IsSuccess = false, ErrorMessage = "Registered Failed" };

        }

        public async Task<AccountResult> LoginUser(string username, string password)
        {
            var result = await _userRepository.GetUserByUsername(username);

            if (result == null)
            {
                _logger.LogWarning("Login failed: User {UserName} not found.", username);

                return new AccountResult { IsSuccess = false, ErrorMessage = "Invalid Credentials" };
            }
            if (!BCrypt.Net.BCrypt.Verify(password, result.HashedPassword))
            {
                _logger.LogWarning("Login failed: Invalid password for {UserName}.", username);

                return new AccountResult { IsSuccess = false, ErrorMessage = "Invalid Credentials!" };
            }

            await _userRepository.SaveLogin(result.Id);

            string token = _jwtHelper.GetJwtToken(new Credential { Id = result.Id, Role = result.Role, UserName = result.UserName });

            bool isAdmin = result.Role == "Admin";

            _logger.LogInformation("User {UserId} logged in successfully.", result.Id);


            return new AccountResult { IsSuccess = true, UserId = result.Id, Token = token, IsAdmin = isAdmin };
        }
    }
}
