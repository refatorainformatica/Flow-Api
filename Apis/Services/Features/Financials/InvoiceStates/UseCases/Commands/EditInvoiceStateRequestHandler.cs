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
    public class EditInvoiceStateRequestHandler(
        IAuthenticatedUserService authenticatedUserService,
        IDateTimeService dateTimeService,
        IMediator mediator,
        InvoiceStateDbContext invoicestateDbContext
    )
        : CommandHandler(invoicestateDbContext, mediator),
            IRequestHandler<EditInvoiceStateRequest, Result<Response<InvoiceStateResponse>>>
    {
        private readonly InvoiceStateDbContext _invoicestateDbContext = invoicestateDbContext;

        private readonly IAuthenticatedUserService _authenticatedUserService =
            authenticatedUserService;

        private readonly IDateTimeService _dateTimeService = dateTimeService;

        public async Task<Result<Response<InvoiceStateResponse>>> Handle(
            EditInvoiceStateRequest request,
            CancellationToken cancellationToken
        )
        {
            return await ValidateRequest(request)
                .BindAsync(req => GetCurrentInvoiceStateAsync(req.Id, cancellationToken))
                .BindAsync(currentInvoiceState =>
                    EditAndSaveInvoiceStateAsync(currentInvoiceState, request, cancellationToken)
                )
                .MapAsync(currentInvoiceState =>
                {
                    return new Response<InvoiceStateResponse>(null);
                });
        }

        private static Result<EditInvoiceStateRequest> ValidateRequest(
            EditInvoiceStateRequest request
        )
        {
            return request.Id != request.RequestId
                ? Result<EditInvoiceStateRequest>.Failure(
                    InvoiceStateErrors.PreConditionFailed(request.RequestId)
                )
                : Result<EditInvoiceStateRequest>.Success(request);
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

        private async Task<Result<InvoiceState>> EditAndSaveInvoiceStateAsync(
            InvoiceState currentInvoiceState,
            EditInvoiceStateRequest request,
            CancellationToken cancellationToken
        )
        {
            var editInvoiceState = new InvoiceState(
                request.Id,
                request.Description,
                request.Picture ?? Shared.Infrastructure.Resources.Images.PictureBase64Image,
                currentInvoiceState.CreatedAt.GetValueOrDefault(),
                currentInvoiceState.CreatedBy,
                _dateTimeService.UtcNow,
                _authenticatedUserService.UserId
            );

            editInvoiceState.AddEvent(new InvoiceStateEditedEvent(editInvoiceState.Id));

            await ExecuteTransactionAsync(
                () => _invoicestateDbContext.InvoiceStates.Update(editInvoiceState),
                editInvoiceState.GetEvents(),
                cancellationToken
            );

            return Result<InvoiceState>.Success(editInvoiceState);
        }
    }
}
