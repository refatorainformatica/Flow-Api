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
    public class EditMovementTypeRequestHandler(
        IAuthenticatedUserService authenticatedUserService,
        IDateTimeService dateTimeService,
        IMediator mediator,
        MovementTypeDbContext movementtypeDbContext
    )
        : CommandHandler(movementtypeDbContext, mediator),
            IRequestHandler<EditMovementTypeRequest, Result<Response<MovementTypeResponse>>>
    {
        private readonly MovementTypeDbContext _movementtypeDbContext = movementtypeDbContext;

        private readonly IAuthenticatedUserService _authenticatedUserService =
            authenticatedUserService;

        private readonly IDateTimeService _dateTimeService = dateTimeService;

        public async Task<Result<Response<MovementTypeResponse>>> Handle(
            EditMovementTypeRequest request,
            CancellationToken cancellationToken
        )
        {
            return await ValidateRequest(request)
                .BindAsync(req => GetCurrentMovementTypeAsync(req.Id, cancellationToken))
                .BindAsync(currentMovementType =>
                    EditAndSaveMovementTypeAsync(currentMovementType, request, cancellationToken)
                )
                .MapAsync(currentMovementType =>
                {
                    return new Response<MovementTypeResponse>(null);
                });
        }

        private static Result<EditMovementTypeRequest> ValidateRequest(
            EditMovementTypeRequest request
        )
        {
            return request.Id != request.RequestId
                ? Result<EditMovementTypeRequest>.Failure(
                    MovementTypeErrors.PreConditionFailed(request.RequestId)
                )
                : Result<EditMovementTypeRequest>.Success(request);
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

        private async Task<Result<MovementType>> EditAndSaveMovementTypeAsync(
            MovementType currentMovementType,
            EditMovementTypeRequest request,
            CancellationToken cancellationToken
        )
        {
            var editMovementType = new MovementType(
                request.Id,
                request.Description,
                request.Picture ?? Shared.Infrastructure.Resources.Images.PictureBase64Image,
                currentMovementType.CreatedAt.GetValueOrDefault(),
                currentMovementType.CreatedBy,
                _dateTimeService.UtcNow,
                _authenticatedUserService.UserId
            );

            editMovementType.AddEvent(new MovementTypeEditedEvent(editMovementType.Id));

            await ExecuteTransactionAsync(
                () => _movementtypeDbContext.MovementTypes.Update(editMovementType),
                editMovementType.GetEvents(),
                cancellationToken
            );

            return Result<MovementType>.Success(editMovementType);
        }
    }
}
