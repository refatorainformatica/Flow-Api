using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Features.Financials.Invoices.Exceptions;
using Services.Features.Financials.Invoices.Models;
using Services.Features.Financials.Invoices.Repositories;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.Primitives;
using Shared.Infrastructure.Extensions;

namespace Services.Features.Financials.Invoices.UseCases.Queries
{
    public class GetInvoiceRequestHandler(
        IMapper mapper,
        IMediator mediator,
        InvoiceDbContext invoiceDbContext
    )
        : CommandHandler(invoiceDbContext, mediator),
            IRequestHandler<GetInvoiceRequest, Result<Response<IEnumerable<InvoiceResponse>>>>
    {
        private readonly InvoiceDbContext _invoiceDbContext = invoiceDbContext;

        public async Task<Result<Response<IEnumerable<InvoiceResponse>>>> Handle(
            GetInvoiceRequest request,
            CancellationToken cancellationToken
        )
        {
            return await GetInvoiceAsync(request)
                .BindAsync(invoices => Task.FromResult(GenerateResponse(invoices)));
        }

        private async Task<Result<Pagination<Invoice>>> GetInvoiceAsync(GetInvoiceRequest request)
        {
            var invoices = await Task.Run(
                () =>
                    _invoiceDbContext
                        .Invoices.AsNoTracking()
                        .Where(x => !x.DeletedAt.HasValue)
                        .SortBy(request.Query.SortBy.ToString(), request.Query.SortOrderAscending)
                        .CreatePagination(request.Query.Offset, request.Query.Limit)
                    ?? new Pagination<Invoice>()
            );

            return !invoices.Rows.Any()
                ? Result<Pagination<Invoice>>.Failure(InvoiceErrors.IsEmpty())
                : Result<Pagination<Invoice>>.Success(invoices);
        }

        private Result<Response<IEnumerable<InvoiceResponse>>> GenerateResponse(
            Pagination<Invoice> paginationInvoice
        )
        {
            var invoiceResponse = mapper.Map<IEnumerable<InvoiceResponse>>(paginationInvoice.Rows);
            var response = new Response<IEnumerable<InvoiceResponse>>(
                invoiceResponse,
                paginationInvoice.Offset,
                paginationInvoice.Limit,
                paginationInvoice.PageCount,
                paginationInvoice.RowCount
            );
            return Result<Response<IEnumerable<InvoiceResponse>>>.Success(response);
        }
    }
}
