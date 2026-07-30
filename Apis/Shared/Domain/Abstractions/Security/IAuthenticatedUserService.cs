using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;

namespace Shared.Domain.Abstractions.Security
{
    public interface IAuthenticatedUserService
    {
        string UserId { get; }

        bool IsAuthenticated();

        void SetAuthenticationStateProvider(
            AuthenticationStateProvider authenticationStateProvider
        );

        Task<bool> IsAuthenticatedAsync();

        Task<string> GetUserNameAsync();
    }
}
