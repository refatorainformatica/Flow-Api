using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Features.Financials.CostCenters.Exceptions;
using Services.Features.Financials.CostCenters.Models;
using Services.Features.Financials.CostCenters.Repositories;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.Primitives;
using Shared.Infrastructure.Extensions;

namespace Services.Features.Financials.CostCenters.UseCases.Queries
{
    public class GetCostCenterRequestHandler(
        IMapper mapper,
        IMediator mediator,
        CostCenterDbContext costcenterDbContext
    )
        : CommandHandler(costcenterDbContext, mediator),
            IRequestHandler<GetCostCenterRequest, Result<Response<IEnumerable<CostCenterResponse>>>>
    {
        private readonly CostCenterDbContext _costcenterDbContext = costcenterDbContext;

        public async Task<Result<Response<IEnumerable<CostCenterResponse>>>> Handle(
            GetCostCenterRequest request,
            CancellationToken cancellationToken
        )
        {
            return await GetCostCenterAsync(request)
                .BindAsync(costcenters => Task.FromResult(GenerateResponse(costcenters)));
        }

        private async Task<Result<Pagination<CostCenter>>> GetCostCenterAsync(
            GetCostCenterRequest request
        )
        {
            var costcenters = await Task.Run(
                () =>
                    _costcenterDbContext
                        .CostCenters.AsNoTracking()
                        .Where(x => !x.DeletedAt.HasValue)
                        .SortBy(request.Query.SortBy.ToString(), request.Query.SortOrderAscending)
                        .CreatePagination(request.Query.Offset, request.Query.Limit)
                    ?? new Pagination<CostCenter>()
            );

            return !costcenters.Rows.Any()
                ? Result<Pagination<CostCenter>>.Failure(CostCenterErrors.IsEmpty())
                : Result<Pagination<CostCenter>>.Success(costcenters);
        }

        private Result<Response<IEnumerable<CostCenterResponse>>> GenerateResponse(
            Pagination<CostCenter> paginationCostCenter
        )
        {
            var costcenterResponse = mapper.Map<IEnumerable<CostCenterResponse>>(
                paginationCostCenter.Rows
            );
            var response = new Response<IEnumerable<CostCenterResponse>>(
                costcenterResponse,
                paginationCostCenter.Offset,
                paginationCostCenter.Limit,
                paginationCostCenter.PageCount,
                paginationCostCenter.RowCount
            );
            return Result<Response<IEnumerable<CostCenterResponse>>>.Success(response);
        }
    }
}
