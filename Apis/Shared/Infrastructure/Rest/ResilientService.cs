using System;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;

namespace Shared.Infrastructure.Rest
{
    public class ResilientService(ILogger<ResilientService> logger)
    {
        public IAsyncPolicy<HttpResponseMessage> BuildExecutionPolicy(string requestUrl)
        {
            var retryPolicy = BuildRetryPolicy(requestUrl);
            var circuitBreakerPolicy = BuildCircuitBreakerPolicy(requestUrl);

            return Policy.WrapAsync(retryPolicy, circuitBreakerPolicy);
        }

        private AsyncRetryPolicy<HttpResponseMessage> BuildRetryPolicy(string requestUrl) =>
            Policy
                .HandleResult<HttpResponseMessage>(r =>
                    r.StatusCode == System.Net.HttpStatusCode.RequestTimeout
                    || !r.IsSuccessStatusCode
                    || r.StatusCode == System.Net.HttpStatusCode.InternalServerError
                )
                .Or<Exception>()
                .WaitAndRetryAsync(
                    3,
                    retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    onRetry: (outcome, timespan, retryCount, context) =>
                    {
                        var statusCode = outcome.Result?.StatusCode.ToString() ?? "Error";
                        logger.LogWarning(
                            "Retry: {RetryCount} - Status Code: {StatusCode} - Calling URL: {RequestUrl} - Waiting {Timespan} seconds before next retry.",
                            retryCount,
                            statusCode,
                            requestUrl,
                            timespan.TotalSeconds
                        );
                    }
                );

        private AsyncCircuitBreakerPolicy<HttpResponseMessage> BuildCircuitBreakerPolicy(
            string requestUrl
        ) =>
            Policy
                .HandleResult<HttpResponseMessage>(r =>
                    r.StatusCode == System.Net.HttpStatusCode.RequestTimeout
                    || !r.IsSuccessStatusCode
                    || r.StatusCode == System.Net.HttpStatusCode.InternalServerError
                )
                .Or<Exception>()
                .CircuitBreakerAsync(
                    5,
                    TimeSpan.FromSeconds(30),
                    onBreak: (outcome, timespan) =>
                    {
                        var statusCode = outcome.Result?.StatusCode.ToString() ?? "Error";
                        logger.LogError(
                            "Circuit Breaker triggered due to status code: {StatusCode}. Calling URL: {RequestUrl}. The circuit is broken for {Timespan} seconds.",
                            statusCode,
                            requestUrl,
                            timespan.TotalSeconds
                        );
                    },
                    onReset: () =>
                    {
                        logger.LogInformation("Circuit Breaker has been reset.");
                    },
                    onHalfOpen: () =>
                    {
                        logger.LogInformation(
                            "Circuit Breaker is half-open, allowing a test request."
                        );
                    }
                );
    }
}
