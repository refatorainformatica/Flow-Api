using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Features.Financials.RevenueTypes.Exceptions;
using Services.Features.Financials.RevenueTypes.Models;
using Services.Features.Financials.RevenueTypes.Repositories;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.Primitives;
using Shared.Infrastructure.Extensions;

namespace Services.Features.Financials.RevenueTypes.UseCases.Queries
{
    public class GetBySearchRevenueTypeRequestHandler(
        IMapper mapper,
        IMediator mediator,
        RevenueTypeDbContext revenuetypeDbContext
    )
        : CommandHandler(revenuetypeDbContext, mediator),
            IRequestHandler<
                GetBySearchRevenueTypeRequest,
                Result<Response<IEnumerable<RevenueTypeResponse>>>
            >
    {
        private readonly RevenueTypeDbContext _revenuetypeDbContext = revenuetypeDbContext;

        public async Task<Result<Response<IEnumerable<RevenueTypeResponse>>>> Handle(
            GetBySearchRevenueTypeRequest request,
            CancellationToken cancellationToken
        )
        {
            return await GetBySearchRevenueTypeAsync(request)
                .BindAsync(revenuetypes => Task.FromResult(GenerateResponse(revenuetypes)));
        }

        private async Task<Result<Pagination<RevenueType>>> GetBySearchRevenueTypeAsync(
            GetBySearchRevenueTypeRequest request
        )
        {
            var revenuetypes = await Task.Run(
                () =>
                    _revenuetypeDbContext
                        .RevenueTypes.AsNoTracking()
                        .SearchBy(request.Query.SearchText)
                        .Where(x => !x.DeletedAt.HasValue)
                        .SortBy(request.Query.SortBy.ToString(), request.Query.SortOrderAscending)
                        .CreatePagination(request.Query.Offset, request.Query.Limit)
                    ?? new Pagination<RevenueType>()
            );

            return !revenuetypes.Rows.Any()
                ? Result<Pagination<RevenueType>>.Failure(
                    RevenueTypeErrors.NotFound(request.Query.SearchText)
                )
                : Result<Pagination<RevenueType>>.Success(revenuetypes);
        }

        private Result<Response<IEnumerable<RevenueTypeResponse>>> GenerateResponse(
            Pagination<RevenueType> paginationRevenueType
        )
        {
            var revenuetypeResponse = mapper.Map<IEnumerable<RevenueTypeResponse>>(
                paginationRevenueType.Rows
            );
            var response = new Response<IEnumerable<RevenueTypeResponse>>(
                revenuetypeResponse,
                paginationRevenueType.Offset,
                paginationRevenueType.Limit,
                paginationRevenueType.PageCount,
                paginationRevenueType.RowCount
            );
            return Result<Response<IEnumerable<RevenueTypeResponse>>>.Success(response);
        }
    }
}
