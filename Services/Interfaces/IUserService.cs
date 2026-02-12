using MvcApp.Models.Common;
using MvcApp.Models.Entities;
using MvcApp.Models.ViewModels;

namespace MvcApp.Services.Interfaces
{
    public interface IUserService
    {
        Task<AccountResult> RegisterUser(RegisterViewModel userRegisterDto);

        Task<AccountResult> LoginUser(string username, string password);

        Task<UserProfileViewModel?> GetUserProfile(int id);

        Task<AccountResult> UpdateUserProfile(int id,UserProfileViewModel profileViewModel);

        Task<AccountResult> ChangePassword(int id, string oldpassword, string password);

    }
}
