using MvcApp.Helper;
using MvcApp.Models.Common;
using MvcApp.Models.ViewModels;
using MvcApp.Repository.Interfaces;
using MvcApp.Services.Interfaces;

namespace MvcApp.Services
{
    public class AdminServices : IAdminServices
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserFinder _userFinder;
        private readonly ILogger<AdminServices> _logger;

        public AdminServices(IUserRepository userRepository,IUserFinder userFinder,ILogger<AdminServices> logger)
        {
            _userRepository = userRepository;
            _userFinder = userFinder;
            _logger = logger;
        }

        public async Task<IEnumerable<UsersViewModel>> GetAllUsers()
        {
            _logger.LogInformation("Fetching AllUsers data");

            var role = _userFinder.GetRole();

            if (role != "Admin")
            {
                _logger.LogWarning("Autharization revoked! for {role}", role);

                throw new UnauthorizedAccessException();
            }

            _logger.LogInformation("Retrived all users data successfully");
            return await _userRepository.GetAllUsers();
        }

        public async Task<AccountResult> DeleteUser(int id)
        {
            _logger.LogInformation("operation started for remove user : {UserId}", id);

            var result = await _userRepository.DeleteUser(id);

            if (result == null)
            {
                _logger.LogError("User Not Found For {userId}", id);

                return new AccountResult { IsSuccess = false, ErrorMessage = "no user found on the corresponding id" };
            }

            if (result.ResultCode == 1)
            {
                _logger.LogInformation("User {UserId} removed successfully ", id);

                return new AccountResult { IsSuccess = true };
            }
            _logger.LogError("operatin failed for user {userId}", id);

            return new AccountResult { IsSuccess = false, ErrorMessage = result.Message ?? "Operation Failed" };
        }

    }
}
