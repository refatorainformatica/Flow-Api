using AutoMapper;
using MediatR;
using Services.Features.Financials.MovementTypes.Models;
using Services.Features.Financials.MovementTypes.Models.Events;
using Services.Features.Financials.MovementTypes.Repositories;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.DateTime;
using Shared.Domain.Abstractions.Primitives;
using Shared.Domain.Abstractions.Security;

namespace Services.Features.Financials.MovementTypes.UseCases.Commands
{
    public class CreateMovementTypeRequestHandler(
        IAuthenticatedUserService authenticatedUserService,
        IDateTimeService dateTimeService,
        IMapper mapper,
        IMediator mediator,
        MovementTypeDbContext movementtypeDbContext
    )
        : CommandHandler(movementtypeDbContext, mediator),
            IRequestHandler<CreateMovementTypeRequest, Result<Response<MovementTypeResponse>>>
    {
        private readonly MovementTypeDbContext _movementtypeDbContext = movementtypeDbContext;

        private readonly IAuthenticatedUserService _authenticatedUserService =
            authenticatedUserService;

        private readonly IDateTimeService _dateTimeService = dateTimeService;

        public async Task<Result<Response<MovementTypeResponse>>> Handle(
            CreateMovementTypeRequest request,
            CancellationToken cancellationToken
        )
        {
            return await SaveMovementTypeAsync(request, cancellationToken)
                .BindAsync(movementtype => Task.FromResult(GenerateResponse(movementtype)));
        }

        private async Task<Result<MovementType>> SaveMovementTypeAsync(
            CreateMovementTypeRequest request,
            CancellationToken cancellationToken
        )
        {
            var newMovementType = new MovementType(
                0,
                request.Description,
                request.Picture ?? Shared.Infrastructure.Resources.Images.PictureBase64Image,
                _dateTimeService.UtcNow,
                _authenticatedUserService.UserId,
                _dateTimeService.UtcNow,
                _authenticatedUserService.UserId
            );

            newMovementType.AddEvent(new MovementTypeCreatedEvent(newMovementType.Id));

            await ExecuteTransactionAsync(
                async () =>
                {
                    await _movementtypeDbContext.MovementTypes.AddAsync(
                        newMovementType,
                        cancellationToken: cancellationToken
                    );
                },
                newMovementType.GetEvents(),
                cancellationToken: cancellationToken
            );

            return Result<MovementType>.Success(newMovementType);
        }

        private Result<Response<MovementTypeResponse>> GenerateResponse(MovementType movementtype)
        {
            var movementtypeResponse = mapper.Map<MovementTypeResponse>(movementtype);
            var response = new Response<MovementTypeResponse>(movementtypeResponse);

            return Result<Response<MovementTypeResponse>>.Success(response);
        }
    }
}
