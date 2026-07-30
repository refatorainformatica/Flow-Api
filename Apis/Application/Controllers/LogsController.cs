using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog.Events;
using Serilog.Parsing;

namespace Application.Controllers
{
    /// <summary>
    /// LogController provides endpoints for retrieving log about the application.
    /// </summary>
    [AllowAnonymous]
    [ApiVersion("1.0")]
    [Tags("Logs Endpoints")]
    [ApiExplorerSettings(GroupName = "Logs")]
    [Route("api/v{version:apiVersion}/logs")]
    public class LogsController(ILogger<LogsController> logger) : BaseApiController
    {
        private readonly ILogger<LogsController> _logger = logger;

        /// <summary>
        /// Retrieves information about the application's version, last update time, and environment.
        /// </summary>
        /// <returns>A string containing the application's version, last update time, and environment.</returns>
        [HttpPost()]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        public async Task<IActionResult> ReceiveLogsAsync()
        {
            // O fluxo de dados do corpo da requisição
            using (var memoryStream = new MemoryStream())
            {
                // Copiar o conteúdo da requisição para a memória
                await Request.Body.CopyToAsync(memoryStream);

                // Agora você tem o corpo como um MemoryStream
                // Você pode manipular ou deserializar conforme necessário

                memoryStream.Position = 0; // Voltar para o início do stream
                using (var reader = new StreamReader(memoryStream))
                {
                    var logContent = await reader.ReadToEndAsync();
                    var logEvents = DeserializeLogEvents(logContent);

                    if (logEvents == null || logEvents.Count == 0)
                    {
                        return BadRequest("Invalid log format.");
                    }

                    // Logar com base no nível
                    foreach (var logEvent in logEvents)
                    {
                        LogEventBasedOnLevel(logEvent);
                    }

                    return Ok();
                }
            }
        }

        private void LogEventBasedOnLevel(LogEvent logEvent)
        {
            // Utiliza um switch para verificar o nível de log
            switch (logEvent.Level)
            {
                case LogEventLevel.Debug:
                    _logger.LogDebug(logEvent.MessageTemplate.Render(logEvent.Properties));
                    break;

                case LogEventLevel.Error:
                    _logger.LogError(logEvent.MessageTemplate.Render(logEvent.Properties));
                    break;

                case LogEventLevel.Fatal:
                    _logger.LogError(logEvent.MessageTemplate.Render(logEvent.Properties));
                    break;

                case LogEventLevel.Information:
                    _logger.LogInformation(logEvent.MessageTemplate.Render(logEvent.Properties));
                    break;

                case LogEventLevel.Verbose:
                    _logger.LogTrace(logEvent.MessageTemplate.Render(logEvent.Properties));
                    break;

                case LogEventLevel.Warning:
                    _logger.LogWarning(logEvent.MessageTemplate.Render(logEvent.Properties));
                    break;

                default:
                    // Caso o nível de log seja desconhecido, pode-se optar por um log genérico ou outro comportamento.
                    _logger.LogInformation(logEvent.MessageTemplate.Render(logEvent.Properties));
                    break;
            }
        }

        private static List<LogEvent> DeserializeLogEvents(string json)
        {
            var logEvents = new List<LogEvent>();
            var jsonElement = JsonSerializer.Deserialize<JsonElement>(json);

            // Verifica se é um array ou um único objeto
            if (jsonElement.ValueKind == JsonValueKind.Array)
            {
                foreach (var element in jsonElement.EnumerateArray())
                {
                    logEvents.Add(ParseLogEvent(element));
                }
            }
            else
            {
                logEvents.Add(ParseLogEvent(jsonElement));
            }

            return logEvents;
        }

        private static LogEvent ParseLogEvent(JsonElement jsonObject)
        {
            var timestamp = jsonObject.GetProperty("Timestamp").GetDateTime();
            var level = Enum.Parse<LogEventLevel>(jsonObject.GetProperty("Level").GetString());
            var messageTemplate = jsonObject
                .GetProperty("MessageTemplate")
                .GetProperty("Text")
                .GetString();

            // Processar as propriedades dinamicamente
            var properties = new Dictionary<string, LogEventPropertyValue>();
            if (jsonObject.TryGetProperty("Properties", out JsonElement propertiesElement))
            {
                foreach (var property in propertiesElement.EnumerateObject())
                {
                    properties[property.Name] = new ScalarValue(property.Value.ToString());
                }
            }

            return new LogEvent(
                timestamp,
                level,
                null, // Exceção (se aplicável)
                new MessageTemplate(messageTemplate, Enumerable.Empty<MessageTemplateToken>()),
                properties.Select(kvp => new LogEventProperty(kvp.Key, kvp.Value)).ToList()
            );
        }
    }
}
