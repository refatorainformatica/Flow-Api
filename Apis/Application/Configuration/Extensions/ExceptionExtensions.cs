using Application.Configuration.Exceptions;
using Shared.Infrastructure.Resources;

namespace Application.Configuration.Extensions
{
    /// <summary>
    /// Provides extension methods for handling exceptions in the application.
    /// </summary>
    public static class ExceptionExtensions
    {
        /// <summary>
        /// Adds exception handling and problem details configuration to the service collection.
        /// </summary>
        /// <param name="services">The service collection to configure.</param>
        public static void AddExceptionExtension(this IServiceCollection services)
        {
            services.AddExceptionHandler<GlobalExceptionHandler>();

            services.AddProblemDetails(options =>
            {
                options.CustomizeProblemDetails = (
                    context =>
                    {
                        if (context.ProblemDetails.Status == StatusCodes.Status401Unauthorized)
                        {
                            context.ProblemDetails.Title = "Unauthorized";
                            context.ProblemDetails.Detail =
                                "You must be authenticated to access this resource.";
                            context.ProblemDetails.Type =
                                Config.HttpResponseErrorTypeStatus401Unauthorized;
                        }
                    }
                );
            });
        }
    }
}
