using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Shared.Domain.Abstractions.Enumerations;
using Shared.Domain.Abstractions.Primitives;

namespace Shared.Domain.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken
        )
        {
            if (_validators.Any())
            {
                var context = new ValidationContext<TRequest>(request);
                var validationResults = await Task.WhenAll(
                    _validators.Select(v => v.ValidateAsync(context, cancellationToken))
                );

                var failures = validationResults
                    .SelectMany(r => r.Errors)
                    .Where(f => f != null)
                    .ToList();

                if (failures.Any())
                {
                    if (
                        typeof(TResponse).IsGenericType
                        && typeof(TResponse).GetGenericTypeDefinition() == typeof(Result<>)
                    )
                    {
                        var errorResult = (TResponse)
                            typeof(Result<>)
                                .MakeGenericType(typeof(TResponse).GetGenericArguments()[0])
                                .GetMethod("Failure", [typeof(List<Error>)])
                                .Invoke(
                                    null,
                                    [
                                        failures
                                            .Select(f => new Error(
                                                ErrorType.Validation,
                                                f.PropertyName,
                                                f.ErrorMessage
                                            ))
                                            .ToList(),
                                    ]
                                );

                        return errorResult;
                    }

                    throw new ValidationException(failures);
                }
            }
            return await next();
        }
    }
}
