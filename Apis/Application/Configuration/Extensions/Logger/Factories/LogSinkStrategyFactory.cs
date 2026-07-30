using Application.Configuration.Extensions.Logger.Strategies;
using Shared.Domain.Abstractions.Enumerations;

namespace Application.Configuration.Extensions.Logger.Factories
{
    /// <summary>
    /// Factory class for creating instances of log sink strategies based on the specified logger provider.
    /// </summary>
    public static class LogSinkStrategyFactory
    {
        /// <summary>
        /// Creates an instance of a log sink strategy based on the specified logger provider.
        /// </summary>
        /// <param name="provider">The logger provider to determine the log sink strategy.</param>
        /// <returns>An instance of a class implementing <see cref="ILogSinkStrategy"/>.</returns>
        public static ILogSinkStrategy Create(LoggerProvider provider) =>
            provider switch
            {
                LoggerProvider.Console => new ConsoleLogSinkStrategy(),
                LoggerProvider.Debug => new DebugLogSinkStrategy(),
                LoggerProvider.File => new FileLogSinkStrategy(),
                LoggerProvider.Seq => new SeqLogSinkStrategy(),
                LoggerProvider.ElasticSearch => new ElasticSearchLogSinkStrategy(),
                LoggerProvider.ApplicationInsights => new ApplicationInsightsLogSinkStrategy(),
                _ => new ConsoleLogSinkStrategy(),
            };
    }
}
