using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Features.Financials.CostCenters.Exceptions;
using Services.Features.Financials.CostCenters.Models;
using Services.Features.Financials.CostCenters.Models.Events;
using Services.Features.Financials.CostCenters.Repositories;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.DateTime;
using Shared.Domain.Abstractions.Primitives;
using Shared.Domain.Abstractions.Security;

namespace Services.Features.Financials.CostCenters.UseCases.Commands
{
    public class RemoveCostCenterRequestHandler(
        IAuthenticatedUserService authenticatedUserService,
        IDateTimeService dateTimeService,
        IMediator mediator,
        CostCenterDbContext costcenterDbContext
    )
        : CommandHandler(costcenterDbContext, mediator),
            IRequestHandler<RemoveCostCenterRequest, Result<Response<CostCenterResponse>>>
    {
        private readonly CostCenterDbContext _costcenterDbContext = costcenterDbContext;

        private readonly IAuthenticatedUserService _authenticatedUserService =
            authenticatedUserService;

        private readonly IDateTimeService _dateTimeService = dateTimeService;

        public async Task<Result<Response<CostCenterResponse>>> Handle(
            RemoveCostCenterRequest request,
            CancellationToken cancellationToken
        )
        {
            return await ValidateRequest(request)
                .BindAsync(req => GetCurrentCostCenterAsync(req.Id, cancellationToken))
                .BindAsync(currentCostCenter =>
                    RemoveCostCenterAsync(currentCostCenter, cancellationToken)
                )
                .MapAsync(currentCostCenter =>
                {
                    return new Response<CostCenterResponse>(null);
                });
        }

        private static Result<RemoveCostCenterRequest> ValidateRequest(
            RemoveCostCenterRequest request
        )
        {
            return request.Id == default
                ? Result<RemoveCostCenterRequest>.Failure(CostCenterErrors.NotFound(request.Id))
                : Result<RemoveCostCenterRequest>.Success(request);
        }

        private async Task<Result<CostCenter>> GetCurrentCostCenterAsync(
            int id,
            CancellationToken cancellationToken
        )
        {
            var costcenter = await _costcenterDbContext
                .CostCenters.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id && !x.DeletedAt.HasValue, cancellationToken);

            return costcenter is null
                ? Result<CostCenter>.Failure(CostCenterErrors.NotFound(id))
                : Result<CostCenter>.Success(costcenter);
        }

        private async Task<Result<CostCenter>> RemoveCostCenterAsync(
            CostCenter removeCostCenter,
            CancellationToken cancellationToken
        )
        {
            removeCostCenter.DeletedAt = _dateTimeService.UtcNow;
            removeCostCenter.EditedAt = _dateTimeService.UtcNow;
            removeCostCenter.EditedBy = _authenticatedUserService.UserId;

            removeCostCenter.AddEvent(new CostCenterRemovedEvent(removeCostCenter.Id));

            await ExecuteTransactionAsync(
                () => _costcenterDbContext.Update(removeCostCenter),
                removeCostCenter.GetEvents(),
                cancellationToken
            );

            return Result<CostCenter>.Success(removeCostCenter);
        }
    }
}
