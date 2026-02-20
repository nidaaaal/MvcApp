using MvcApp.Models.Common;
using MvcApp.Models.Entities;
using MvcApp.Models.ViewModels;

namespace MvcApp.Repository.Interfaces
{
    public interface IUserRepository
    {
        Task<int> RegisterUser(RegisterViewModel dto, string hashedpass, int age,DateTime dob);
        Task<Credential?> GetUserByUsername(string username);
        Task SaveLogin(int id);
        Task<Users?> GetUserProfile(int id);
        Task<DbResponse> UpdateUserProfile(int id,ProfileUpdateViewModel updateProfileDto,int age, string role);
        Task<string?> GetPasswordById(int id);
        Task<bool> SavePassword(int id, string password);
        Task<bool> UploadImage(int id,byte[] imageBytes,string imagePath, string role);
        Task<IEnumerable<UsersViewModel>> GetAllUsers();
        Task<string?> GetImagePath(int id);
        Task<DbResponse?> DeleteUser(int id);
    }
}
