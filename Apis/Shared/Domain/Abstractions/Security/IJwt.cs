using System.Collections.Generic;
using System.Security.Claims;

namespace Shared.Domain.Abstractions.Security
{
    public interface IJwt
    {
        string CreateToken(string key, List<Claim> claims);
    }
}
