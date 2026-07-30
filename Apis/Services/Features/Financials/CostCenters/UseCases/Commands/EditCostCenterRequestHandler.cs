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
    public class EditCostCenterRequestHandler(
        IAuthenticatedUserService authenticatedUserService,
        IDateTimeService dateTimeService,
        IMediator mediator,
        CostCenterDbContext costcenterDbContext
    )
        : CommandHandler(costcenterDbContext, mediator),
            IRequestHandler<EditCostCenterRequest, Result<Response<CostCenterResponse>>>
    {
        private readonly CostCenterDbContext _costcenterDbContext = costcenterDbContext;

        private readonly IAuthenticatedUserService _authenticatedUserService =
            authenticatedUserService;

        private readonly IDateTimeService _dateTimeService = dateTimeService;

        public async Task<Result<Response<CostCenterResponse>>> Handle(
            EditCostCenterRequest request,
            CancellationToken cancellationToken
        )
        {
            return await ValidateRequest(request)
                .BindAsync(req => GetCurrentCostCenterAsync(req.Id, cancellationToken))
                .BindAsync(currentCostCenter =>
                    EditAndSaveCostCenterAsync(currentCostCenter, request, cancellationToken)
                )
                .MapAsync(currentCostCenter =>
                {
                    return new Response<CostCenterResponse>(null);
                });
        }

        private static Result<EditCostCenterRequest> ValidateRequest(EditCostCenterRequest request)
        {
            return request.Id != request.RequestId
                ? Result<EditCostCenterRequest>.Failure(
                    CostCenterErrors.PreConditionFailed(request.RequestId)
                )
                : Result<EditCostCenterRequest>.Success(request);
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

        private async Task<Result<CostCenter>> EditAndSaveCostCenterAsync(
            CostCenter currentCostCenter,
            EditCostCenterRequest request,
            CancellationToken cancellationToken
        )
        {
            var editCostCenter = new CostCenter(
                request.Id,
                request.Description,
                request.Picture ?? Shared.Infrastructure.Resources.Images.PictureBase64Image,
                currentCostCenter.CreatedAt.GetValueOrDefault(),
                currentCostCenter.CreatedBy,
                _dateTimeService.UtcNow,
                _authenticatedUserService.UserId
            );

            editCostCenter.AddEvent(new CostCenterEditedEvent(editCostCenter.Id));

            await ExecuteTransactionAsync(
                () => _costcenterDbContext.CostCenters.Update(editCostCenter),
                editCostCenter.GetEvents(),
                cancellationToken
            );

            return Result<CostCenter>.Success(editCostCenter);
        }
    }
}
