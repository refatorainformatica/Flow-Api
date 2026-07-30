using Application.Configuration.Extensions.Logger.Strategies;
using Serilog;
using Serilog.Events;

namespace Application.Configuration.Extensions.Logger
{
    /// <summary>
    /// Provides a strategy for configuring Application Insights as a log sink.
    /// </summary>
    public class ApplicationInsightsLogSinkStrategy : ILogSinkStrategy
    {
        /// <summary>
        /// Configures the logger to use Application Insights as a log sink.
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
            var instrumentationKey = configuration[
                "Observability:ApplicationInsights:InstrumentationKey"
            ];
            if (!string.IsNullOrWhiteSpace(instrumentationKey))
            {
                loggerConfiguration.WriteTo.ApplicationInsights(
                    instrumentationKey,
                    TelemetryConverter.Traces,
                    restrictedToMinimumLevel: LogEventLevel.Information
                );
            }
        }
    }
}
