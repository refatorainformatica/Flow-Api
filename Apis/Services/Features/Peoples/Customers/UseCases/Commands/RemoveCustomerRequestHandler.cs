using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Features.Peoples.Customers.Exceptions;
using Services.Features.Peoples.Customers.Models;
using Services.Features.Peoples.Customers.Models.Events;
using Services.Features.Peoples.Customers.Repositories;
using Services.Features.Peoples.Customers.UseCases.Commands;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.DateTime;
using Shared.Domain.Abstractions.Primitives;
using Shared.Domain.Abstractions.Security;

namespace Services.Features.Financials.Customers.UseCases.Commands
{
    public class RemoveCustomerRequestHandler(
        IAuthenticatedUserService authenticatedUserService,
        IDateTimeService dateTimeService,
        IMediator mediator,
        CustomerDbContext customerDbContext
    )
        : CommandHandler(customerDbContext, mediator),
            IRequestHandler<RemoveCustomerRequest, Result<Response<CustomerResponse>>>
    {
        private readonly CustomerDbContext _customerDbContext = customerDbContext;

        private readonly IAuthenticatedUserService _authenticatedUserService =
            authenticatedUserService;

        private readonly IDateTimeService _dateTimeService = dateTimeService;

        public async Task<Result<Response<CustomerResponse>>> Handle(
            RemoveCustomerRequest request,
            CancellationToken cancellationToken
        )
        {
            return await ValidateRequest(request)
                .BindAsync(req => GetCurrentCustomerAsync(req.Id, cancellationToken))
                .BindAsync(currentCustomer =>
                    RemoveCustomerAsync(currentCustomer, cancellationToken)
                )
                .MapAsync(currentCustomer =>
                {
                    return new Response<CustomerResponse>(null);
                });
        }

        private static Result<RemoveCustomerRequest> ValidateRequest(RemoveCustomerRequest request)
        {
            return request.Id == default
                ? Result<RemoveCustomerRequest>.Failure(CustomerErrors.NotFound(request.Id))
                : Result<RemoveCustomerRequest>.Success(request);
        }

        private async Task<Result<Customer>> GetCurrentCustomerAsync(
            int id,
            CancellationToken cancellationToken
        )
        {
            var customer = await _customerDbContext
                .Customers.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id && !x.DeletedAt.HasValue, cancellationToken);

            return customer is null
                ? Result<Customer>.Failure(CustomerErrors.NotFound(id))
                : Result<Customer>.Success(customer);
        }

        private async Task<Result<Customer>> RemoveCustomerAsync(
            Customer removeCustomer,
            CancellationToken cancellationToken
        )
        {
            removeCustomer.DeletedAt = _dateTimeService.UtcNow;
            removeCustomer.EditedAt = _dateTimeService.UtcNow;
            removeCustomer.EditedBy = _authenticatedUserService.UserId;

            removeCustomer.AddEvent(new CustomerRemovedEvent(removeCustomer.Id));

            await ExecuteTransactionAsync(
                () => _customerDbContext.Update(removeCustomer),
                removeCustomer.GetEvents(),
                cancellationToken
            );

            return Result<Customer>.Success(removeCustomer);
        }
    }
}
