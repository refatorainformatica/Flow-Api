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
    public class EditInvoiceRequestHandler(
        IAuthenticatedUserService authenticatedUserService,
        IDateTimeService dateTimeService,
        IMediator mediator,
        InvoiceDbContext invoiceDbContext
    )
        : CommandHandler(invoiceDbContext, mediator),
            IRequestHandler<EditInvoiceRequest, Result<Response<InvoiceResponse>>>
    {
        private readonly InvoiceDbContext _invoiceDbContext = invoiceDbContext;

        private readonly IAuthenticatedUserService _authenticatedUserService =
            authenticatedUserService;

        private readonly IDateTimeService _dateTimeService = dateTimeService;

        public async Task<Result<Response<InvoiceResponse>>> Handle(
            EditInvoiceRequest request,
            CancellationToken cancellationToken
        )
        {
            return await ValidateRequest(request)
                .BindAsync(req => GetCurrentInvoiceAsync(req.Id, cancellationToken))
                .BindAsync(currentInvoice =>
                    EditAndSaveInvoiceAsync(currentInvoice, request, cancellationToken)
                )
                .MapAsync(currentInvoice =>
                {
                    return new Response<InvoiceResponse>(null);
                });
        }

        private static Result<EditInvoiceRequest> ValidateRequest(EditInvoiceRequest request)
        {
            return request.Id != request.RequestId
                ? Result<EditInvoiceRequest>.Failure(
                    InvoiceErrors.PreConditionFailed(request.RequestId)
                )
                : Result<EditInvoiceRequest>.Success(request);
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

        private async Task<Result<Invoice>> EditAndSaveInvoiceAsync(
            Invoice currentInvoice,
            EditInvoiceRequest request,
            CancellationToken cancellationToken
        )
        {
            var editInvoice = new Invoice(
                request.Id,
                request.SupplierId,
                request.InvoiceTypeId,
                request.InvoiceStateId,
                request.File,
                request.Picture ?? Shared.Infrastructure.Resources.Images.PictureBase64Image,
                currentInvoice.CreatedAt.GetValueOrDefault(),
                currentInvoice.CreatedBy,
                _dateTimeService.UtcNow,
                _authenticatedUserService.UserId
            );

            editInvoice.AddEvent(new InvoiceEditedEvent(editInvoice.Id));

            await ExecuteTransactionAsync(
                () => _invoiceDbContext.Invoices.Update(editInvoice),
                editInvoice.GetEvents(),
                cancellationToken
            );

            return Result<Invoice>.Success(editInvoice);
        }
    }
}
