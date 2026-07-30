using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Features.Financials.Invoices.Exceptions;
using Services.Features.Financials.Invoices.Models;
using Services.Features.Financials.Invoices.Repositories;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.Invoices.UseCases.Queries
{
    public class GetByIdInvoiceRequestHandler(
        IMapper mapper,
        IMediator mediator,
        InvoiceDbContext invoiceDbContext
    )
        : CommandHandler(invoiceDbContext, mediator),
            IRequestHandler<GetByIdInvoiceRequest, Result<Response<InvoiceResponse>>>
    {
        private readonly InvoiceDbContext _invoiceDbContext = invoiceDbContext;

        public async Task<Result<Response<InvoiceResponse>>> Handle(
            GetByIdInvoiceRequest request,
            CancellationToken cancellationToken
        )
        {
            return await GetByIdInvoiceAsync(request, cancellationToken)
                .BindAsync(invoices => Task.FromResult(GenerateResponse(invoices)));
        }

        private async Task<Result<Invoice>> GetByIdInvoiceAsync(
            GetByIdInvoiceRequest request,
            CancellationToken cancellationToken
        )
        {
            var invoice = await _invoiceDbContext
                .Invoices.AsNoTracking()
                .FirstOrDefaultAsync(
                    x => x.Id == request.Id && !x.DeletedAt.HasValue,
                    cancellationToken
                );

            return invoice is null
                ? Result<Invoice>.Failure(InvoiceErrors.NotFound(request.Id))
                : Result<Invoice>.Success(invoice);
        }

        private Result<Response<InvoiceResponse>> GenerateResponse(Invoice invoice)
        {
            var invoiceResponse = mapper.Map<InvoiceResponse>(invoice);
            var response = new Response<InvoiceResponse>(invoiceResponse);
            return Result<Response<InvoiceResponse>>.Success(response);
        }
    }
}
