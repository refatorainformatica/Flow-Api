using Serilog;

namespace Application.Configuration.Extensions.Logger.Strategies
{
    /// <summary>
    /// Provides a strategy for configuring console logging sinks.
    /// </summary>
    public class ConsoleLogSinkStrategy : ILogSinkStrategy
    {
        /// <summary>
        /// Configures the logger with a console sink.
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
            loggerConfiguration.WriteTo.Console(
                outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}"
            );
        }
    }
}
