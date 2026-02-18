using MvcApp.Models.Entities;

namespace MvcApp.Helper
{
    public interface IJwtHelper
    {
        string GetJwtToken(Credential user);

    }
}
