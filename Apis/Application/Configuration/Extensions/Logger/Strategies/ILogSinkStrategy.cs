using Serilog;

namespace Application.Configuration.Extensions.Logger.Strategies
{
    /// <summary>
    /// Defines a strategy for configuring log sinks in the application.
    /// </summary>
    public interface ILogSinkStrategy
    {
        /// <summary>
        /// Configures the logger with the specified settings.
        /// </summary>
        /// <param name="loggerConfiguration">The logger configuration to modify.</param>
        /// <param name="builder">The web application builder.</param>
        /// <param name="configuration">The application configuration.</param>
        void Configure(
            LoggerConfiguration loggerConfiguration,
            WebApplicationBuilder builder,
            IConfiguration configuration
        );
    }
}
