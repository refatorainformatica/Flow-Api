using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Features.Financials.InvoiceStates.Exceptions;
using Services.Features.Financials.InvoiceStates.Models;
using Services.Features.Financials.InvoiceStates.Repositories;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.Primitives;
using Shared.Infrastructure.Extensions;

namespace Services.Features.Financials.InvoiceStates.UseCases.Queries
{
    public class GetInvoiceStateRequestHandler(
        IMapper mapper,
        IMediator mediator,
        InvoiceStateDbContext invoicestateDbContext
    )
        : CommandHandler(invoicestateDbContext, mediator),
            IRequestHandler<
                GetInvoiceStateRequest,
                Result<Response<IEnumerable<InvoiceStateResponse>>>
            >
    {
        private readonly InvoiceStateDbContext _invoicestateDbContext = invoicestateDbContext;

        public async Task<Result<Response<IEnumerable<InvoiceStateResponse>>>> Handle(
            GetInvoiceStateRequest request,
            CancellationToken cancellationToken
        )
        {
            return await GetInvoiceStateAsync(request)
                .BindAsync(invoicestates => Task.FromResult(GenerateResponse(invoicestates)));
        }

        private async Task<Result<Pagination<InvoiceState>>> GetInvoiceStateAsync(
            GetInvoiceStateRequest request
        )
        {
            var invoicestates = await Task.Run(
                () =>
                    _invoicestateDbContext
                        .InvoiceStates.AsNoTracking()
                        .Where(x => !x.DeletedAt.HasValue)
                        .SortBy(request.Query.SortBy.ToString(), request.Query.SortOrderAscending)
                        .CreatePagination(request.Query.Offset, request.Query.Limit)
                    ?? new Pagination<InvoiceState>()
            );

            return !invoicestates.Rows.Any()
                ? Result<Pagination<InvoiceState>>.Failure(InvoiceStateErrors.IsEmpty())
                : Result<Pagination<InvoiceState>>.Success(invoicestates);
        }

        private Result<Response<IEnumerable<InvoiceStateResponse>>> GenerateResponse(
            Pagination<InvoiceState> paginationInvoiceState
        )
        {
            var invoicestateResponse = mapper.Map<IEnumerable<InvoiceStateResponse>>(
                paginationInvoiceState.Rows
            );
            var response = new Response<IEnumerable<InvoiceStateResponse>>(
                invoicestateResponse,
                paginationInvoiceState.Offset,
                paginationInvoiceState.Limit,
                paginationInvoiceState.PageCount,
                paginationInvoiceState.RowCount
            );
            return Result<Response<IEnumerable<InvoiceStateResponse>>>.Success(response);
        }
    }
}
