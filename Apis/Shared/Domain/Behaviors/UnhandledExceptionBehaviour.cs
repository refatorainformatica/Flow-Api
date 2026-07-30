using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Shared.Domain.Behaviors
{
    [ExcludeFromCodeCoverage]
    public class UnhandledExceptionBehaviour<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
    {
        private readonly ILogger<UnhandledExceptionBehaviour<TRequest, TResponse>> _logger;

        public UnhandledExceptionBehaviour(
            ILogger<UnhandledExceptionBehaviour<TRequest, TResponse>> logger
        )
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken
        )
        {
            try
            {
                return await next();
            }
            catch (Exception ex)
            {
                var requestName = typeof(TRequest).Name;
                var message =
                    $"Request: Unhandled Exception for Request {requestName} {request} {ex.Message}";
                _logger.LogError(ex, message);

                throw new InvalidOperationException(ex.Message, ex);
            }
        }
    }
}
