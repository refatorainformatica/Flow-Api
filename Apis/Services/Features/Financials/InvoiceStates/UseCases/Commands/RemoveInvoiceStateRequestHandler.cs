using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Features.Financials.InvoiceStates.Exceptions;
using Services.Features.Financials.InvoiceStates.Models;
using Services.Features.Financials.InvoiceStates.Models.Events;
using Services.Features.Financials.InvoiceStates.Repositories;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.DateTime;
using Shared.Domain.Abstractions.Primitives;
using Shared.Domain.Abstractions.Security;

namespace Services.Features.Financials.InvoiceStates.UseCases.Commands
{
    public class RemoveInvoiceStateRequestHandler(
        IAuthenticatedUserService authenticatedUserService,
        IDateTimeService dateTimeService,
        IMediator mediator,
        InvoiceStateDbContext invoicestateDbContext
    )
        : CommandHandler(invoicestateDbContext, mediator),
            IRequestHandler<RemoveInvoiceStateRequest, Result<Response<InvoiceStateResponse>>>
    {
        private readonly InvoiceStateDbContext _invoicestateDbContext = invoicestateDbContext;

        private readonly IAuthenticatedUserService _authenticatedUserService =
            authenticatedUserService;

        private readonly IDateTimeService _dateTimeService = dateTimeService;

        public async Task<Result<Response<InvoiceStateResponse>>> Handle(
            RemoveInvoiceStateRequest request,
            CancellationToken cancellationToken
        )
        {
            return await ValidateRequest(request)
                .BindAsync(req => GetCurrentInvoiceStateAsync(req.Id, cancellationToken))
                .BindAsync(currentInvoiceState =>
                    RemoveInvoiceStateAsync(currentInvoiceState, cancellationToken)
                )
                .MapAsync(currentInvoiceState =>
                {
                    return new Response<InvoiceStateResponse>(null);
                });
        }

        private static Result<RemoveInvoiceStateRequest> ValidateRequest(
            RemoveInvoiceStateRequest request
        )
        {
            return request.Id == default
                ? Result<RemoveInvoiceStateRequest>.Failure(InvoiceStateErrors.NotFound(request.Id))
                : Result<RemoveInvoiceStateRequest>.Success(request);
        }

        private async Task<Result<InvoiceState>> GetCurrentInvoiceStateAsync(
            int id,
            CancellationToken cancellationToken
        )
        {
            var invoicestate = await _invoicestateDbContext
                .InvoiceStates.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id && !x.DeletedAt.HasValue, cancellationToken);

            return invoicestate is null
                ? Result<InvoiceState>.Failure(InvoiceStateErrors.NotFound(id))
                : Result<InvoiceState>.Success(invoicestate);
        }

        private async Task<Result<InvoiceState>> RemoveInvoiceStateAsync(
            InvoiceState removeInvoiceState,
            CancellationToken cancellationToken
        )
        {
            removeInvoiceState.DeletedAt = _dateTimeService.UtcNow;
            removeInvoiceState.EditedAt = _dateTimeService.UtcNow;
            removeInvoiceState.EditedBy = _authenticatedUserService.UserId;

            removeInvoiceState.AddEvent(new InvoiceStateRemovedEvent(removeInvoiceState.Id));

            await ExecuteTransactionAsync(
                () => _invoicestateDbContext.Update(removeInvoiceState),
                removeInvoiceState.GetEvents(),
                cancellationToken
            );

            return Result<InvoiceState>.Success(removeInvoiceState);
        }
    }
}
