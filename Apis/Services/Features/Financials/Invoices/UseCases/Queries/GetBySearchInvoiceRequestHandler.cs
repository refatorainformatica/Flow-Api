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
    public class GetBySearchInvoiceRequestHandler(
        IMapper mapper,
        IMediator mediator,
        InvoiceDbContext invoiceDbContext
    )
        : CommandHandler(invoiceDbContext, mediator),
            IRequestHandler<
                GetBySearchInvoiceRequest,
                Result<Response<IEnumerable<InvoiceResponse>>>
            >
    {
        private readonly InvoiceDbContext _invoiceDbContext = invoiceDbContext;

        public async Task<Result<Response<IEnumerable<InvoiceResponse>>>> Handle(
            GetBySearchInvoiceRequest request,
            CancellationToken cancellationToken
        )
        {
            return await GetBySearchInvoiceAsync(request)
                .BindAsync(invoices => Task.FromResult(GenerateResponse(invoices)));
        }

        private async Task<Result<Pagination<Invoice>>> GetBySearchInvoiceAsync(
            GetBySearchInvoiceRequest request
        )
        {
            var invoices = await Task.Run(
                () =>
                    _invoiceDbContext
                        .Invoices.AsNoTracking()
                        .SearchBy(request.Query.SearchText)
                        .Where(x => !x.DeletedAt.HasValue)
                        .SortBy(request.Query.SortBy.ToString(), request.Query.SortOrderAscending)
                        .CreatePagination(request.Query.Offset, request.Query.Limit)
                    ?? new Pagination<Invoice>()
            );

            return !invoices.Rows.Any()
                ? Result<Pagination<Invoice>>.Failure(
                    InvoiceErrors.NotFound(request.Query.SearchText)
                )
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
