using Microsoft.AspNetCore.Authorization;
using Shared.Domain.Abstractions.Enumerations;

namespace Application.Configuration.Extensions;

/// <summary>
/// Provides extension methods for configuring authorization services.
/// </summary>
public static class AuthorizationExtensions
{
    /// <summary>
    /// Configures authorization services with default policies and authentication schemes.
    /// </summary>
    /// <param name="services">The service collection to add authorization to.</param>
    /// <param name="configuration">The application configuration containing authentication settings.</param>
    public static void ConfigureAuthorization(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var authorizationProvider =
            configuration["Security:Authorization:DefalutProvider"]
            ?? AuthorizationProvider.Jwt.ToString();

        var authenticationScheme = authorizationProvider switch
        {
            var provider when provider == AuthorizationProvider.AzureAd.ToString() => "AzureAd",
            var provider when provider == AuthorizationProvider.Jwt.ToString() => "Jwt",
            var provider when provider == AuthorizationProvider.Google.ToString() => "Google",
            var provider when provider == AuthorizationProvider.Facebook.ToString() => "Facebook",
            var provider when provider == AuthorizationProvider.Twitter.ToString() => "Twitter",
            var provider when provider == AuthorizationProvider.Microsoft.ToString() => "Microsoft",
            var provider when provider == AuthorizationProvider.LinkedIn.ToString() => "LinkedIn",
            var provider when provider == AuthorizationProvider.GitHub.ToString() => "GitHub",
            var provider when provider == AuthorizationProvider.KeyCloak.ToString() => "KeyCloak",
            _ => "Jwt",
        };

        services.AddAuthorization(options =>
        {
            options.DefaultPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .AddAuthenticationSchemes(authenticationScheme)
                .Build();
        });
    }
}
