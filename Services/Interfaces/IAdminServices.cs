using MvcApp.Models.ViewModels;

namespace MvcApp.Services.Interfaces
{
    public interface IAdminServices
    {
        Task<IEnumerable<UsersViewModel>> GetAllUsers();
    }
}
