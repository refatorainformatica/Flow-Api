using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Shared.Domain.Exceptions;
using Shared.Infrastructure.Resources;

namespace Application.Configuration.Exceptions
{
    /// <summary>
    /// Handles global exceptions and formats them into a standardized response.
    /// </summary>
    /// <param name="env">The hosting environment.</param>
    /// <param name="logger">The logger instance for logging exceptions.</param>
    public class GlobalExceptionHandler(
        IHostEnvironment env,
        ILogger<GlobalExceptionHandler> logger
    ) : IExceptionHandler
    {
        private const string UnhandledExceptionMsg =
            "An unhandled exception has occurred while executing the request.";

        private static readonly JsonSerializerOptions SerializerOptions = new(
            JsonSerializerDefaults.Web
        )
        {
            Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) },
        };

        /// <summary>
        /// Attempts to handle the given exception and format it into a standardized response.
        /// </summary>
        /// <param name="context">The HTTP context of the current request.</param>
        /// <param name="exception">The exception to handle.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating whether the exception was handled.</returns>
        public async ValueTask<bool> TryHandleAsync(
            HttpContext context,
            Exception exception,
            CancellationToken cancellationToken
        )
        {
            //exception.AddErrorCode();

            //If your logger logs DiagnosticsTelemetry, you should remove the string below to avoid the exception being logged twice.
            logger.LogError(
                exception,
                exception is ApiException ? exception.Message : UnhandledExceptionMsg
            );

            var problemDetails = CreateProblemDetails(context, exception);
            var json = ToJson(problemDetails);

            const string contentType = "application/problem+json";
            context.Response.ContentType = contentType;
            await context.Response.WriteAsync(json, cancellationToken);

            return true;
        }

        private ProblemDetails CreateProblemDetails(in HttpContext context, in Exception exception)
        {
            var errorCode = exception.GetHashCode();
            var statusCode = context.Response.StatusCode;
            var reasonPhrase = ReasonPhrases.GetReasonPhrase(statusCode);
            if (string.IsNullOrEmpty(reasonPhrase))
            {
                reasonPhrase = UnhandledExceptionMsg;
            }

            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = reasonPhrase,
                Extensions = { [nameof(errorCode)] = errorCode },
                Type = Config.HttpResponseErrorTypeStatus500InternalServerError,
            };

            if (!env.IsDevelopment())
            {
                return problemDetails;
            }

            problemDetails.Detail = exception.ToString();
            problemDetails.Extensions["traceId"] = context.TraceIdentifier;
            problemDetails.Extensions["data"] = exception.Data;

            return problemDetails;
        }

        private string ToJson(in ProblemDetails problemDetails)
        {
            try
            {
                return JsonSerializer.Serialize(problemDetails, SerializerOptions);
            }
            catch (Exception ex)
            {
                const string msg = "An exception has occurred while serializing error to JSON";
                logger.LogError(ex, msg);
            }

            return string.Empty;
        }
    }
}
