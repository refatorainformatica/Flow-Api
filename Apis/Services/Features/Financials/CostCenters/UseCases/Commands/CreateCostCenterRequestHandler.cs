using AutoMapper;
using MediatR;
using Services.Features.Financials.CostCenters.Models;
using Services.Features.Financials.CostCenters.Models.Events;
using Services.Features.Financials.CostCenters.Repositories;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.DateTime;
using Shared.Domain.Abstractions.Primitives;
using Shared.Domain.Abstractions.Security;

namespace Services.Features.Financials.CostCenters.UseCases.Commands
{
    public class CreateCostCenterRequestHandler(
        IAuthenticatedUserService authenticatedUserService,
        IDateTimeService dateTimeService,
        IMapper mapper,
        IMediator mediator,
        CostCenterDbContext costcenterDbContext
    )
        : CommandHandler(costcenterDbContext, mediator),
            IRequestHandler<CreateCostCenterRequest, Result<Response<CostCenterResponse>>>
    {
        private readonly CostCenterDbContext _costcenterDbContext = costcenterDbContext;

        private readonly IAuthenticatedUserService _authenticatedUserService =
            authenticatedUserService;

        private readonly IDateTimeService _dateTimeService = dateTimeService;

        public async Task<Result<Response<CostCenterResponse>>> Handle(
            CreateCostCenterRequest request,
            CancellationToken cancellationToken
        )
        {
            return await SaveCostCenterAsync(request, cancellationToken)
                .BindAsync(costcenter => Task.FromResult(GenerateResponse(costcenter)));
        }

        private async Task<Result<CostCenter>> SaveCostCenterAsync(
            CreateCostCenterRequest request,
            CancellationToken cancellationToken
        )
        {
            var newCostCenter = new CostCenter(
                0,
                request.Description,
                request.Picture ?? Shared.Infrastructure.Resources.Images.PictureBase64Image,
                _dateTimeService.UtcNow,
                _authenticatedUserService.UserId,
                _dateTimeService.UtcNow,
                _authenticatedUserService.UserId
            );

            newCostCenter.AddEvent(new CostCenterCreatedEvent(newCostCenter.Id));

            await ExecuteTransactionAsync(
                async () =>
                {
                    await _costcenterDbContext.CostCenters.AddAsync(
                        newCostCenter,
                        cancellationToken: cancellationToken
                    );
                },
                newCostCenter.GetEvents(),
                cancellationToken: cancellationToken
            );

            return Result<CostCenter>.Success(newCostCenter);
        }

        private Result<Response<CostCenterResponse>> GenerateResponse(CostCenter costcenter)
        {
            var costcenterResponse = mapper.Map<CostCenterResponse>(costcenter);
            var response = new Response<CostCenterResponse>(costcenterResponse);

            return Result<Response<CostCenterResponse>>.Success(response);
        }
    }
}
