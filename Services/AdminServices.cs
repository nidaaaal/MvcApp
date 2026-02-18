using MvcApp.Helper;
using MvcApp.Models.ViewModels;
using MvcApp.Repository.Interfaces;
using MvcApp.Services.Interfaces;

namespace MvcApp.Services
{
    public class AdminServices : IAdminServices
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserFinder _userFinder;

        public AdminServices(IUserRepository userRepository,IUserFinder userFinder)
        {
            _userRepository = userRepository;
            _userFinder = userFinder;
        }

        public async Task<IEnumerable<UsersViewModel>> GetAllUsers()
        {
            var role = _userFinder.GetRole();

            if (role != "Admin") throw new UnauthorizedAccessException();

            return await _userRepository.GetAllUsers();
        }

    }
}
