using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Shared.Domain.Behaviors
{
    [ExcludeFromCodeCoverage]
    public class PerformanceBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
    {
        private readonly Stopwatch _timer;
        private readonly ILogger<PerformanceBehaviour<TRequest, TResponse>> _logger;

        public PerformanceBehaviour(ILogger<PerformanceBehaviour<TRequest, TResponse>> logger)
        {
            _timer = new Stopwatch();
            _logger = logger;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken
        )
        {
            _timer.Start();

            var response = await next();

            _timer.Stop();

            var elapsedMilliseconds = _timer.ElapsedMilliseconds;

            if (elapsedMilliseconds > 500)
            {
                var requestName = typeof(TRequest).Name;
                var message =
                    $"Long Running Request: {requestName} ({elapsedMilliseconds} milliseconds) {request}";
                _logger.LogInformation(message);
            }

            return response;
        }
    }
}
