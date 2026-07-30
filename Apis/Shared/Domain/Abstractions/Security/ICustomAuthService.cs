using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shared.Domain.Abstractions.Security
{
    public interface ICustomAuthService
    {
        Task SetCustomUserClaimsAsync(string userId, Dictionary<string, object> claims);

        Task<string> GetTokenAsync(string userId);
    }
}
