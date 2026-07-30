using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Features.Peoples.MaritalStates.Exceptions;
using Services.Features.Peoples.MaritalStates.Models;
using Services.Features.Peoples.MaritalStates.Models.Events;
using Services.Features.Peoples.MaritalStates.Repositories;
using Services.Features.Peoples.MaritalStates.UseCases.Commands;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.DateTime;
using Shared.Domain.Abstractions.Primitives;
using Shared.Domain.Abstractions.Security;

namespace Services.Features.Financials.MaritalStates.UseCases.Commands
{
    public class EditMaritalStateRequestHandler(
        IAuthenticatedUserService authenticatedUserService,
        IDateTimeService dateTimeService,
        IMediator mediator,
        MaritalStateDbContext maritalstateDbContext
    )
        : CommandHandler(maritalstateDbContext, mediator),
            IRequestHandler<EditMaritalStateRequest, Result<Response<MaritalStateResponse>>>
    {
        private readonly MaritalStateDbContext _maritalstateDbContext = maritalstateDbContext;

        private readonly IAuthenticatedUserService _authenticatedUserService =
            authenticatedUserService;

        private readonly IDateTimeService _dateTimeService = dateTimeService;

        public async Task<Result<Response<MaritalStateResponse>>> Handle(
            EditMaritalStateRequest request,
            CancellationToken cancellationToken
        )
        {
            return await ValidateRequest(request)
                .BindAsync(req => GetCurrentMaritalStateAsync(req.Id, cancellationToken))
                .BindAsync(currentMaritalState =>
                    EditAndSaveMaritalStateAsync(currentMaritalState, request, cancellationToken)
                )
                .MapAsync(currentMaritalState =>
                {
                    return new Response<MaritalStateResponse>(null);
                });
        }

        private static Result<EditMaritalStateRequest> ValidateRequest(
            EditMaritalStateRequest request
        )
        {
            return request.Id != request.RequestId
                ? Result<EditMaritalStateRequest>.Failure(
                    MaritalStateErrors.PreConditionFailed(request.RequestId)
                )
                : Result<EditMaritalStateRequest>.Success(request);
        }

        private async Task<Result<MaritalState>> GetCurrentMaritalStateAsync(
            int id,
            CancellationToken cancellationToken
        )
        {
            var maritalstate = await _maritalstateDbContext
                .MaritalStates.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id && !x.DeletedAt.HasValue, cancellationToken);

            return maritalstate is null
                ? Result<MaritalState>.Failure(MaritalStateErrors.NotFound(id))
                : Result<MaritalState>.Success(maritalstate);
        }

        private async Task<Result<MaritalState>> EditAndSaveMaritalStateAsync(
            MaritalState currentMaritalState,
            EditMaritalStateRequest request,
            CancellationToken cancellationToken
        )
        {
            var editMaritalState = new MaritalState(
                request.Id,
                request.Description,
                request.Picture ?? Shared.Infrastructure.Resources.Images.PictureBase64Image,
                currentMaritalState.CreatedAt.GetValueOrDefault(),
                currentMaritalState.CreatedBy,
                _dateTimeService.UtcNow,
                _authenticatedUserService.UserId
            );

            editMaritalState.AddEvent(new MaritalStateEditedEvent(editMaritalState.Id));

            await ExecuteTransactionAsync(
                () => _maritalstateDbContext.MaritalStates.Update(editMaritalState),
                editMaritalState.GetEvents(),
                cancellationToken
            );

            return Result<MaritalState>.Success(editMaritalState);
        }
    }
}
