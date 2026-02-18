using Microsoft.AspNetCore.Http;

namespace MvcApp.Helper
{
    public class UserFinder : IUserFinder
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserFinder(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int GetId()
        {
            if ( _httpContextAccessor.HttpContext?.Items["userId"] is int userId)
            {
                return userId;
            }

            throw new Exception("No User found on The corresponding Id");
        }
        public string GetRole()
        {
            if (_httpContextAccessor.HttpContext?.Items["userRole"] is string role)
            {
                return role;
            }

            throw new Exception("No User found on The corresponding Id");

        }
    }
}