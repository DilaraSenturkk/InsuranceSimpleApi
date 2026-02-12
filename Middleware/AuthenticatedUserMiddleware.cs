using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using InsuranceSimpleApi.Models;

namespace InsuranceSimpleApi.Middleware
{
    public class AuthenticatedUserMiddleware : IMiddleware
    {
        private readonly AuthenticatedUser _user;

        public AuthenticatedUserMiddleware(AuthenticatedUser user)
        {
            _user = user;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (context.User.Identity?.IsAuthenticated == true)
            {
                _user.IsAuthenticated = true;
                _user.UserId = int.Parse(
                    context.User.FindFirstValue(ClaimTypes.NameIdentifier)!
                );
                _user.Name = context.User.FindFirstValue(ClaimTypes.Name);
                _user.Role = context.User.FindFirstValue(ClaimTypes.Role);
            }

            await next(context);
        }
    }
}
