using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using Shared.Domain.Abstractions.Security;
using Shared.Domain.Exceptions;

namespace Shared.Infrastructure
{
    public class AuthenticatedUserService(IHttpContextAccessor httpContextAccessor)
        : IAuthenticatedUserService
    {
        public string UserId
        {
            get { return GetUsername(); }
        }

        private string GetUsername()
        {
            var username = httpContextAccessor.HttpContext?.User?.FindFirst("user_id")?.Value;
            if (string.IsNullOrEmpty(username))
            {
                username = httpContextAccessor.HttpContext?.User?.FindFirstValue("unique_name");
            }
            if (string.IsNullOrEmpty(username))
            {
                username = httpContextAccessor.HttpContext?.User?.Identity?.Name;
            }

            if (string.IsNullOrEmpty(username))
                throw new UnauthorizedException("Not logged user.");

            return username;
        }

        public bool IsAuthenticated()
        {
            if (httpContextAccessor == null)
                return false;

            return (
                httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated
            ).GetValueOrDefault();
        }

        private AuthenticationStateProvider authenticationStateProvider;

        public void SetAuthenticationStateProvider(
            AuthenticationStateProvider authenticationStateProvider
        )
        {
            this.authenticationStateProvider = authenticationStateProvider;
        }

        public async Task<bool> IsAuthenticatedAsync()
        {
            if (authenticationStateProvider == null)
                return false;
            AuthenticationState authState = null;
            try
            {
                authState = await authenticationStateProvider.GetAuthenticationStateAsync();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }

            return (authState?.User?.Identity?.IsAuthenticated).GetValueOrDefault();
        }

        public async Task<string> GetUserNameAsync()
        {
            var username = string.Empty;

            if (authenticationStateProvider != null)
            {
                var authState = await authenticationStateProvider.GetAuthenticationStateAsync();

                username = authState.User.FindFirst("user_id")?.Value;
                if (string.IsNullOrEmpty(username))
                {
                    username = authState.User.FindFirstValue("unique_name");
                }
                if (string.IsNullOrEmpty(username))
                {
                    username = authState.User.Identity?.Name;
                }
            }

            if (string.IsNullOrEmpty(username))
                throw new UnauthorizedException("Not logged user.");

            return username;
        }
    }
}
