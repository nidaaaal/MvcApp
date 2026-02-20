using MvcApp.Models.Common;
using MvcApp.Models.Entities;
using MvcApp.Models.ViewModels;

namespace MvcApp.Services.Interfaces
{
    public interface IUserService
    {

        Task<UserProfileViewModel?> GetUserProfile(int id);

        Task<AccountResult> UpdateUserProfile(int id,UserProfileViewModel profileViewModel,string role);

        Task<AccountResult> ChangePassword(int id, string oldpassword, string password);

        Task<AccountResult> UpdateImage(int id, IFormFile file, string role);

        Task<ProfileImageViewModel> GetProfileImage(int id);

    }
}
