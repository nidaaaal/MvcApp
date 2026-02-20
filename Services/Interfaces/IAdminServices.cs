using MvcApp.Models.Common;
using MvcApp.Models.ViewModels;

namespace MvcApp.Services.Interfaces
{
    public interface IAdminServices
    {
        Task<IEnumerable<UsersViewModel>> GetAllUsers();
        Task<AccountResult> DeleteUser(int id);

    }
}
