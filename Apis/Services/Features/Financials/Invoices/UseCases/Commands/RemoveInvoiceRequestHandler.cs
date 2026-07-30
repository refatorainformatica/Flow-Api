using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Features.Financials.Invoices.Exceptions;
using Services.Features.Financials.Invoices.Models;
using Services.Features.Financials.Invoices.Models.Events;
using Services.Features.Financials.Invoices.Repositories;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.DateTime;
using Shared.Domain.Abstractions.Primitives;
using Shared.Domain.Abstractions.Security;

namespace Services.Features.Financials.Invoices.UseCases.Commands
{
    public class RemoveInvoiceRequestHandler(
        IAuthenticatedUserService authenticatedUserService,
        IDateTimeService dateTimeService,
        IMediator mediator,
        InvoiceDbContext invoiceDbContext
    )
        : CommandHandler(invoiceDbContext, mediator),
            IRequestHandler<RemoveInvoiceRequest, Result<Response<InvoiceResponse>>>
    {
        private readonly InvoiceDbContext _invoiceDbContext = invoiceDbContext;

        private readonly IAuthenticatedUserService _authenticatedUserService =
            authenticatedUserService;

        private readonly IDateTimeService _dateTimeService = dateTimeService;

        public async Task<Result<Response<InvoiceResponse>>> Handle(
            RemoveInvoiceRequest request,
            CancellationToken cancellationToken
        )
        {
            return await ValidateRequest(request)
                .BindAsync(req => GetCurrentInvoiceAsync(req.Id, cancellationToken))
                .BindAsync(currentInvoice => RemoveInvoiceAsync(currentInvoice, cancellationToken))
                .MapAsync(currentInvoice =>
                {
                    return new Response<InvoiceResponse>(null);
                });
        }

        private static Result<RemoveInvoiceRequest> ValidateRequest(RemoveInvoiceRequest request)
        {
            return request.Id == default
                ? Result<RemoveInvoiceRequest>.Failure(InvoiceErrors.NotFound(request.Id))
                : Result<RemoveInvoiceRequest>.Success(request);
        }

        private async Task<Result<Invoice>> GetCurrentInvoiceAsync(
            int id,
            CancellationToken cancellationToken
        )
        {
            var invoice = await _invoiceDbContext
                .Invoices.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id && !x.DeletedAt.HasValue, cancellationToken);

            return invoice is null
                ? Result<Invoice>.Failure(InvoiceErrors.NotFound(id))
                : Result<Invoice>.Success(invoice);
        }

        private async Task<Result<Invoice>> RemoveInvoiceAsync(
            Invoice removeInvoice,
            CancellationToken cancellationToken
        )
        {
            removeInvoice.DeletedAt = _dateTimeService.UtcNow;
            removeInvoice.EditedAt = _dateTimeService.UtcNow;
            removeInvoice.EditedBy = _authenticatedUserService.UserId;

            removeInvoice.AddEvent(new InvoiceRemovedEvent(removeInvoice.Id));

            await ExecuteTransactionAsync(
                () => _invoiceDbContext.Update(removeInvoice),
                removeInvoice.GetEvents(),
                cancellationToken
            );

            return Result<Invoice>.Success(removeInvoice);
        }
    }
}
