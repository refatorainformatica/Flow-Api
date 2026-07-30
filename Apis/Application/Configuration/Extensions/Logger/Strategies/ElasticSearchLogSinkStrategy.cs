using Serilog;
using Serilog.Sinks.Elasticsearch;

namespace Application.Configuration.Extensions.Logger.Strategies
{
    /// <summary>
    /// Provides a strategy for configuring Elasticsearch as a log sink.
    /// </summary>
    public class ElasticSearchLogSinkStrategy : ILogSinkStrategy
    {
        /// <summary>
        /// Configures the logger to use Elasticsearch as a log sink.
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
            var esUrl = configuration["Observability:ElasticSearch:Url"];
            if (!string.IsNullOrWhiteSpace(esUrl))
            {
                loggerConfiguration.WriteTo.Elasticsearch(
                    new ElasticsearchSinkOptions(new Uri(esUrl))
                    {
                        AutoRegisterTemplate = true,
                        AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv8,
                        BatchAction = ElasticOpType.Create,
                        BatchPostingLimit = 50,
                        Period = TimeSpan.FromSeconds(5),
                        ModifyConnectionSettings = connection =>
                            connection.RequestTimeout(TimeSpan.FromSeconds(30)),
                        IndexFormat =
                            $"logs-flow-api-{builder.Environment.EnvironmentName.ToLower().Replace(".", "-")}",
                        FailureCallback = e =>
                            Console.WriteLine(
                                $"Erro ao enviar log para o Elasticsearch: {e.Exception.Message}"
                            ),
                        EmitEventFailure =
                            EmitEventFailureHandling.WriteToSelfLog
                            | EmitEventFailureHandling.RaiseCallback
                            | EmitEventFailureHandling.ThrowException,
                    }
                );
            }
        }
    }
}
