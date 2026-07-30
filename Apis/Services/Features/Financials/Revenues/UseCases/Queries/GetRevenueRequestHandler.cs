using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Features.Financials.Revenues.Exceptions;
using Services.Features.Financials.Revenues.Models;
using Services.Features.Financials.Revenues.Repositories;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.Primitives;
using Shared.Infrastructure.Extensions;

namespace Services.Features.Financials.Revenues.UseCases.Queries
{
    public class GetRevenueRequestHandler(
        IMapper mapper,
        IMediator mediator,
        RevenueDbContext revenueDbContext
    )
        : CommandHandler(revenueDbContext, mediator),
            IRequestHandler<GetRevenueRequest, Result<Response<IEnumerable<RevenueResponse>>>>
    {
        private readonly RevenueDbContext _revenueDbContext = revenueDbContext;

        public async Task<Result<Response<IEnumerable<RevenueResponse>>>> Handle(
            GetRevenueRequest request,
            CancellationToken cancellationToken
        )
        {
            return await GetRevenueAsync(request)
                .BindAsync(revenues => Task.FromResult(GenerateResponse(revenues)));
        }

        private async Task<Result<Pagination<Revenue>>> GetRevenueAsync(GetRevenueRequest request)
        {
            var revenues = await Task.Run(
                () =>
                    _revenueDbContext
                        .Revenues.AsNoTracking()
                        .Where(x => !x.DeletedAt.HasValue)
                        .SortBy(request.Query.SortBy.ToString(), request.Query.SortOrderAscending)
                        .CreatePagination(request.Query.Offset, request.Query.Limit)
                    ?? new Pagination<Revenue>()
            );

            return !revenues.Rows.Any()
                ? Result<Pagination<Revenue>>.Failure(RevenueErrors.IsEmpty())
                : Result<Pagination<Revenue>>.Success(revenues);
        }

        private Result<Response<IEnumerable<RevenueResponse>>> GenerateResponse(
            Pagination<Revenue> paginationRevenue
        )
        {
            var revenueResponse = mapper.Map<IEnumerable<RevenueResponse>>(paginationRevenue.Rows);
            var response = new Response<IEnumerable<RevenueResponse>>(
                revenueResponse,
                paginationRevenue.Offset,
                paginationRevenue.Limit,
                paginationRevenue.PageCount,
                paginationRevenue.RowCount
            );
            return Result<Response<IEnumerable<RevenueResponse>>>.Success(response);
        }
    }
}
