using Serilog;

namespace Application.Configuration.Extensions.Logger.Strategies
{
    /// <summary>
    /// Provides a strategy for configuring a file-based log sink.
    /// </summary>
    public class FileLogSinkStrategy : ILogSinkStrategy
    {
        /// <summary>
        /// Configures the logger to use a file-based log sink.
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
            var filePath = configuration["Observability:File:Path"];
            var rollingInterval = Enum.TryParse<RollingInterval>(
                configuration["Observability:File:RollingInterval"],
                out var interval
            )
                ? interval
                : RollingInterval.Day;

            loggerConfiguration.WriteTo.File(
                path: filePath,
                rollingInterval: rollingInterval,
                outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}",
                retainedFileCountLimit: 7
            );
        }
    }
}
