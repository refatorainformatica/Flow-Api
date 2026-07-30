using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Features.Financials.InvoiceTypes.Exceptions;
using Services.Features.Financials.InvoiceTypes.Models;
using Services.Features.Financials.InvoiceTypes.Repositories;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.Primitives;
using Shared.Infrastructure.Extensions;

namespace Services.Features.Financials.InvoiceTypes.UseCases.Queries
{
    public class GetInvoiceTypeRequestHandler(
        IMapper mapper,
        IMediator mediator,
        InvoiceTypeDbContext invoicetypeDbContext
    )
        : CommandHandler(invoicetypeDbContext, mediator),
            IRequestHandler<
                GetInvoiceTypeRequest,
                Result<Response<IEnumerable<InvoiceTypeResponse>>>
            >
    {
        private readonly InvoiceTypeDbContext _invoicetypeDbContext = invoicetypeDbContext;

        public async Task<Result<Response<IEnumerable<InvoiceTypeResponse>>>> Handle(
            GetInvoiceTypeRequest request,
            CancellationToken cancellationToken
        )
        {
            return await GetInvoiceTypeAsync(request)
                .BindAsync(invoicetypes => Task.FromResult(GenerateResponse(invoicetypes)));
        }

        private async Task<Result<Pagination<InvoiceType>>> GetInvoiceTypeAsync(
            GetInvoiceTypeRequest request
        )
        {
            var invoicetypes = await Task.Run(
                () =>
                    _invoicetypeDbContext
                        .InvoiceTypes.AsNoTracking()
                        .Where(x => !x.DeletedAt.HasValue)
                        .SortBy(request.Query.SortBy.ToString(), request.Query.SortOrderAscending)
                        .CreatePagination(request.Query.Offset, request.Query.Limit)
                    ?? new Pagination<InvoiceType>()
            );

            return !invoicetypes.Rows.Any()
                ? Result<Pagination<InvoiceType>>.Failure(InvoiceTypeErrors.IsEmpty())
                : Result<Pagination<InvoiceType>>.Success(invoicetypes);
        }

        private Result<Response<IEnumerable<InvoiceTypeResponse>>> GenerateResponse(
            Pagination<InvoiceType> paginationInvoiceType
        )
        {
            var invoicetypeResponse = mapper.Map<IEnumerable<InvoiceTypeResponse>>(
                paginationInvoiceType.Rows
            );
            var response = new Response<IEnumerable<InvoiceTypeResponse>>(
                invoicetypeResponse,
                paginationInvoiceType.Offset,
                paginationInvoiceType.Limit,
                paginationInvoiceType.PageCount,
                paginationInvoiceType.RowCount
            );
            return Result<Response<IEnumerable<InvoiceTypeResponse>>>.Success(response);
        }
    }
}
