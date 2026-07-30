using Serilog;

namespace Application.Configuration.Extensions.Logger.Strategies
{
    /// <summary>
    /// Provides a strategy for configuring a debug log sink using Serilog.
    /// </summary>
    public class DebugLogSinkStrategy : ILogSinkStrategy
    {
        /// <summary>
        /// Configures the debug log sink using the provided logger configuration.
        /// </summary>
        /// <param name="loggerConfiguration">The logger configuration to modify.</param>
        /// <param name="builder">The web application builder.</param>
        /// <param name="configuration">The application configuration.</param>
        public void Configure(
            LoggerConfiguration loggerConfiguration,
            WebApplicationBuilder builder,
            IConfiguration configuration
        )
        {
            loggerConfiguration.WriteTo.Debug(
                outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}"
            );
        }
    }
}
