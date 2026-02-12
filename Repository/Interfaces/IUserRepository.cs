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
        Task<DbResponse> UpdateUserProfile(int id,ProfileUpdateViewModel updateProfileDto,int age);
        Task<string?> GetPasswordById(int id);
        Task<bool> SavePassword(int id, string password);
    }
}
