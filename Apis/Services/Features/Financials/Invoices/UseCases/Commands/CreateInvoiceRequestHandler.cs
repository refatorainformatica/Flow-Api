using AutoMapper;
using MediatR;
using Services.Features.Financials.Invoices.Models;
using Services.Features.Financials.Invoices.Models.Events;
using Services.Features.Financials.Invoices.Repositories;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.DateTime;
using Shared.Domain.Abstractions.Primitives;
using Shared.Domain.Abstractions.Security;

namespace Services.Features.Financials.Invoices.UseCases.Commands
{
    public class CreateInvoiceRequestHandler(
        IAuthenticatedUserService authenticatedUserService,
        IDateTimeService dateTimeService,
        IMapper mapper,
        IMediator mediator,
        InvoiceDbContext invoiceDbContext
    )
        : CommandHandler(invoiceDbContext, mediator),
            IRequestHandler<CreateInvoiceRequest, Result<Response<InvoiceResponse>>>
    {
        private readonly InvoiceDbContext _invoiceDbContext = invoiceDbContext;

        private readonly IAuthenticatedUserService _authenticatedUserService =
            authenticatedUserService;

        private readonly IDateTimeService _dateTimeService = dateTimeService;

        public async Task<Result<Response<InvoiceResponse>>> Handle(
            CreateInvoiceRequest request,
            CancellationToken cancellationToken
        )
        {
            return await SaveInvoiceAsync(request, cancellationToken)
                .BindAsync(invoice => Task.FromResult(GenerateResponse(invoice)));
        }

        private async Task<Result<Invoice>> SaveInvoiceAsync(
            CreateInvoiceRequest request,
            CancellationToken cancellationToken
        )
        {
            var newInvoice = new Invoice(
                0,
                request.SupplierId,
                request.InvoiceTypeId,
                request.InvoiceStateId,
                request.File,
                request.Picture ?? Shared.Infrastructure.Resources.Images.PictureBase64Image,
                _dateTimeService.UtcNow,
                _authenticatedUserService.UserId,
                _dateTimeService.UtcNow,
                _authenticatedUserService.UserId
            );

            newInvoice.AddEvent(new InvoiceCreatedEvent(newInvoice.Id));

            await ExecuteTransactionAsync(
                async () =>
                {
                    await _invoiceDbContext.Invoices.AddAsync(
                        newInvoice,
                        cancellationToken: cancellationToken
                    );
                },
                newInvoice.GetEvents(),
                cancellationToken: cancellationToken
            );

            return Result<Invoice>.Success(newInvoice);
        }

        private Result<Response<InvoiceResponse>> GenerateResponse(Invoice invoice)
        {
            var invoiceResponse = mapper.Map<InvoiceResponse>(invoice);
            var response = new Response<InvoiceResponse>(invoiceResponse);

            return Result<Response<InvoiceResponse>>.Success(response);
        }
    }
}
