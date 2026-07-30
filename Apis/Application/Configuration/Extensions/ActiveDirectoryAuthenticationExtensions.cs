using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Application.Configuration.Extensions
{
    /// <summary>
    /// Provides extension methods for configuring Active Directory authentication.
    /// </summary>
    public static class ActiveDirectoryAuthenticationExtensions
    {
        /// <summary>
        /// Configures Active Directory authentication for the application.
        /// </summary>
        /// <param name="services">The service collection to add authentication services to.</param>
        /// <param name="configuration">The application configuration containing authentication settings.</param>
        public static void ConfigureActiveADicrectoryAuthentication(
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
                    "AzureAd",
                    options =>
                    {
                        options.RequireHttpsMetadata = false;
                        options.SaveToken = true;
                        options.Authority = configuration[
                            "Security:Authorization:AzureAd:Authority"
                        ];
                        options.Audience = configuration["Security:Authorization:AzureAd:Audience"];

                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuer = true,
                        };
                    }
                );
        }
    }
}
