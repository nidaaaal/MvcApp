using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MvcApp.Middleware
{
    public class UserIdMiddleware
    {
        private readonly RequestDelegate _next;

        public UserIdMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            if (httpContext.User.Identity?.IsAuthenticated == true)
            {
                var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);
                if(userIdClaim!=null && int.TryParse(userIdClaim.Value,out int userId)){

                    httpContext.Items["userId"] = userId;
                }
            }

            if(httpContext.User.Identity?.IsAuthenticated == true)
            {
                var userRoleClaims = httpContext.User.FindFirst(ClaimTypes.Role);
                if(userRoleClaims != null)
                {
                    httpContext.Items["userRole"] = userRoleClaims.Value;
                }
            }


           await _next(httpContext);
        }
    }

    public static class UserIdMiddlewareExtensions
    {
        public static IApplicationBuilder UseUserIdMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<UserIdMiddleware>();
        }
    }
}
