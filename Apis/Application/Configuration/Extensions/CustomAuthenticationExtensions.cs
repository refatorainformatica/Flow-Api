using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Application.Configuration.Extensions
{
    /// <summary>
    /// Provides extension methods for configuring custom authentication.
    /// </summary>
    public static class CustomAuthenticationExtensions
    {
        /// <summary>
        /// Configures custom authentication using JWT bearer tokens.
        /// </summary>
        /// <param name="services">The service collection to add authentication to.</param>
        /// <param name="configuration">The application configuration containing authentication settings.</param>
        public static void ConfigureCustomAuthentication(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            services
                .AddAuthentication(options =>
                {
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(
                    "Jwt",
                    options =>
                    {
                        options.RequireHttpsMetadata = false;
                        options.SaveToken = true;
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = new SymmetricSecurityKey(
                                Encoding.Default.GetBytes(
                                    configuration["Security:Authorization:Jwt:SecurityKey"]
                                )
                            ),
                        };
                    }
                );
        }
    }
}
