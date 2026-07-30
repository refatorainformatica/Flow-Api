using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Features.Financials.MovementTypes.Exceptions;
using Services.Features.Financials.MovementTypes.Models;
using Services.Features.Financials.MovementTypes.Models.Events;
using Services.Features.Financials.MovementTypes.Repositories;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.DateTime;
using Shared.Domain.Abstractions.Primitives;
using Shared.Domain.Abstractions.Security;

namespace Services.Features.Financials.MovementTypes.UseCases.Commands
{
    public class RemoveMovementTypeRequestHandler(
        IAuthenticatedUserService authenticatedUserService,
        IDateTimeService dateTimeService,
        IMediator mediator,
        MovementTypeDbContext movementtypeDbContext
    )
        : CommandHandler(movementtypeDbContext, mediator),
            IRequestHandler<RemoveMovementTypeRequest, Result<Response<MovementTypeResponse>>>
    {
        private readonly MovementTypeDbContext _movementtypeDbContext = movementtypeDbContext;

        private readonly IAuthenticatedUserService _authenticatedUserService =
            authenticatedUserService;

        private readonly IDateTimeService _dateTimeService = dateTimeService;

        public async Task<Result<Response<MovementTypeResponse>>> Handle(
            RemoveMovementTypeRequest request,
            CancellationToken cancellationToken
        )
        {
            return await ValidateRequest(request)
                .BindAsync(req => GetCurrentMovementTypeAsync(req.Id, cancellationToken))
                .BindAsync(currentMovementType =>
                    RemoveMovementTypeAsync(currentMovementType, cancellationToken)
                )
                .MapAsync(currentMovementType =>
                {
                    return new Response<MovementTypeResponse>(null);
                });
        }

        private static Result<RemoveMovementTypeRequest> ValidateRequest(
            RemoveMovementTypeRequest request
        )
        {
            return request.Id == default
                ? Result<RemoveMovementTypeRequest>.Failure(MovementTypeErrors.NotFound(request.Id))
                : Result<RemoveMovementTypeRequest>.Success(request);
        }

        private async Task<Result<MovementType>> GetCurrentMovementTypeAsync(
            int id,
            CancellationToken cancellationToken
        )
        {
            var movementtype = await _movementtypeDbContext
                .MovementTypes.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id && !x.DeletedAt.HasValue, cancellationToken);

            return movementtype is null
                ? Result<MovementType>.Failure(MovementTypeErrors.NotFound(id))
                : Result<MovementType>.Success(movementtype);
        }

        private async Task<Result<MovementType>> RemoveMovementTypeAsync(
            MovementType removeMovementType,
            CancellationToken cancellationToken
        )
        {
            removeMovementType.DeletedAt = _dateTimeService.UtcNow;
            removeMovementType.EditedAt = _dateTimeService.UtcNow;
            removeMovementType.EditedBy = _authenticatedUserService.UserId;

            removeMovementType.AddEvent(new MovementTypeRemovedEvent(removeMovementType.Id));

            await ExecuteTransactionAsync(
                () => _movementtypeDbContext.Update(removeMovementType),
                removeMovementType.GetEvents(),
                cancellationToken
            );

            return Result<MovementType>.Success(removeMovementType);
        }
    }
}
