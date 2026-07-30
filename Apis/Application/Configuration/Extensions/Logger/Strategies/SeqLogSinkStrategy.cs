using Serilog;

namespace Application.Configuration.Extensions.Logger.Strategies
{
    /// <summary>
    /// Provides a strategy for configuring Serilog to use Seq as a log sink.
    /// </summary>
    public class SeqLogSinkStrategy : ILogSinkStrategy
    {
        /// <summary>
        /// Configures the Serilog logger to use Seq as a log sink.
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
            var seqUrl = configuration["Observability:Seq:Url"];
            if (!string.IsNullOrWhiteSpace(seqUrl))
            {
                loggerConfiguration.WriteTo.Seq(seqUrl);
            }
        }
    }
}
