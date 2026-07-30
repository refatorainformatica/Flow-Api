using Application.Configuration.Extensions.Logger.Factories;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Filters;
using Shared.Domain.Abstractions.Enumerations;

namespace Application.Configuration.Extensions
{
    /// <summary>
    /// Provides extension methods for configuring logging in the application.
    /// </summary>
    public static class LoggerExtensions
    {
        /// <summary>
        /// Configures the Serilog logger for the application.
        /// </summary>
        /// <param name="builder">The WebApplicationBuilder used to configure the application.</param>
        /// <param name="configuration">The configuration used to retrieve logging settings.</param>
        public static void ConfigureLogger(
            this WebApplicationBuilder builder,
            IConfiguration configuration
        )
        {
            var loggerProvider = Enum.TryParse<LoggerProvider>(
                configuration["Observability:DefalutProvider"],
                ignoreCase: true,
                out var provider
            )
                ? provider
                : LoggerProvider.Console;

            var loggerConfiguration = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .Enrich.WithExceptionDetails()
                .Enrich.WithCorrelationId()
                .Enrich.WithProperty(
                    "ApplicationName",
                    $"Flow API - {Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT")}"
                )
                .Filter.ByExcluding(Matching.FromSource("Microsoft.AspNetCore.StaticFiles"))
                .WriteTo.Debug(
                    outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}"
                );

            ConfigureLogSink(loggerProvider, builder, configuration, loggerConfiguration);

            var logger = loggerConfiguration.CreateLogger();

            Serilog.Debugging.SelfLog.Enable(msg => Console.WriteLine($"[Serilog] {msg}"));
            builder.Host.UseSerilog(logger);
        }

        private static void ConfigureLogSink(
            LoggerProvider loggerProvider,
            WebApplicationBuilder builder,
            IConfiguration configuration,
            LoggerConfiguration loggerConfiguration
        )
        {
            var logSinkStrategy = LogSinkStrategyFactory.Create(loggerProvider);
            logSinkStrategy.Configure(loggerConfiguration, builder, configuration);
        }
    }
}
