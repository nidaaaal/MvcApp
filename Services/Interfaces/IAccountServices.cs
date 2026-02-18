using MvcApp.Models.Common;
using MvcApp.Models.ViewModels;

namespace MvcApp.Services.Interfaces
{
    public interface IAccountServices
    {

        Task<AccountResult> RegisterUser(RegisterViewModel userRegisterDto);

        Task<AccountResult> LoginUser(string username, string password);
    }
}
