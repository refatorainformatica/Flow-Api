using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Features.Financials.CostCenters.Exceptions;
using Services.Features.Financials.CostCenters.Models;
using Services.Features.Financials.CostCenters.Repositories;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.CostCenters.UseCases.Queries
{
    public class GetByIdCostCenterRequestHandler(
        IMapper mapper,
        IMediator mediator,
        CostCenterDbContext costcenterDbContext
    )
        : CommandHandler(costcenterDbContext, mediator),
            IRequestHandler<GetByIdCostCenterRequest, Result<Response<CostCenterResponse>>>
    {
        private readonly CostCenterDbContext _costcenterDbContext = costcenterDbContext;

        public async Task<Result<Response<CostCenterResponse>>> Handle(
            GetByIdCostCenterRequest request,
            CancellationToken cancellationToken
        )
        {
            return await GetByIdCostCenterAsync(request, cancellationToken)
                .BindAsync(costcenters => Task.FromResult(GenerateResponse(costcenters)));
        }

        private async Task<Result<CostCenter>> GetByIdCostCenterAsync(
            GetByIdCostCenterRequest request,
            CancellationToken cancellationToken
        )
        {
            var costcenter = await _costcenterDbContext
                .CostCenters.AsNoTracking()
                .FirstOrDefaultAsync(
                    x => x.Id == request.Id && !x.DeletedAt.HasValue,
                    cancellationToken
                );

            return costcenter is null
                ? Result<CostCenter>.Failure(CostCenterErrors.NotFound(request.Id))
                : Result<CostCenter>.Success(costcenter);
        }

        private Result<Response<CostCenterResponse>> GenerateResponse(CostCenter costcenter)
        {
            var costcenterResponse = mapper.Map<CostCenterResponse>(costcenter);
            var response = new Response<CostCenterResponse>(costcenterResponse);
            return Result<Response<CostCenterResponse>>.Success(response);
        }
    }
}
