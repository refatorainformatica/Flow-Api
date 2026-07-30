using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Microsoft.IdentityModel.Tokens;
using Shared.Domain.Abstractions.Security;
using Shared.Domain.Exceptions;

namespace Shared.Infrastructure.Firebase
{
    public class FirebaseAuthService : ICustomAuthService
    {
        private readonly FirebaseApp _firebaseApp;
        private readonly IJwt _jwt;
        private readonly FirebaseSecurityAccount _firebaseSecurityAccount;

        public FirebaseAuthService(
            FirebaseApp firebaseApp,
            IJwt jwt,
            FirebaseSecurityAccount firebaseSecurityAccount
        )
        {
            _firebaseApp = firebaseApp;
            _jwt = jwt;
            _firebaseSecurityAccount = firebaseSecurityAccount;
        }

        public async Task SetCustomUserClaimsAsync(string userId, Dictionary<string, object> claims)
        {
            var firebaseAuth = FirebaseAuth.GetAuth(_firebaseApp);
            await firebaseAuth.SetCustomUserClaimsAsync(userId, claims);
        }

        public async Task<string> GetTokenAsync(string userId)
        {
            var firebaseAuth = FirebaseAuth.GetAuth(_firebaseApp);
            var user =
                await firebaseAuth.GetUserAsync(userId)
                ?? throw new ApiException("Not logged user.");

            var claims = new List<Claim>
            {
                new Claim(
                    "iss",
                    $"https://securetoken.google.com/{_firebaseSecurityAccount.ProjectId}",
                    ClaimValueTypes.String
                ),
                new Claim("aud", _firebaseSecurityAccount.ProjectId, ClaimValueTypes.String),
                new Claim("user_id", user.Uid, ClaimValueTypes.String),
                new Claim("sub", user.Uid, ClaimValueTypes.String),
                new Claim(
                    "iat",
                    EpochTime.GetIntDate(DateTime.Now).ToString(),
                    ClaimValueTypes.Integer64
                ),
                new Claim(
                    "exp",
                    EpochTime.GetIntDate(DateTime.Now.AddHours(1)).ToString(),
                    ClaimValueTypes.Integer64
                ),
                new Claim("phone_number", user.PhoneNumber, ClaimValueTypes.String),
                new Claim("sign_in_provider", "Phone", ClaimValueTypes.String),
            };

            var privateKey = _firebaseSecurityAccount
                .PrivateKey.Replace("-----BEGIN PRIVATE KEY-----", string.Empty)
                .Replace("-----END PRIVATE KEY-----", string.Empty)
                .Replace(Environment.NewLine, string.Empty);

            return _jwt.CreateToken(privateKey, claims);
        }
    }
}
