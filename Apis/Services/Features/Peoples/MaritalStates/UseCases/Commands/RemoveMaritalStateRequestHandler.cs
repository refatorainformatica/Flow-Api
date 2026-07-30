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
    public class RemoveMaritalStateRequestHandler(
        IAuthenticatedUserService authenticatedUserService,
        IDateTimeService dateTimeService,
        IMediator mediator,
        MaritalStateDbContext maritalstateDbContext
    )
        : CommandHandler(maritalstateDbContext, mediator),
            IRequestHandler<RemoveMaritalStateRequest, Result<Response<MaritalStateResponse>>>
    {
        private readonly MaritalStateDbContext _maritalstateDbContext = maritalstateDbContext;

        private readonly IAuthenticatedUserService _authenticatedUserService =
            authenticatedUserService;

        private readonly IDateTimeService _dateTimeService = dateTimeService;

        public async Task<Result<Response<MaritalStateResponse>>> Handle(
            RemoveMaritalStateRequest request,
            CancellationToken cancellationToken
        )
        {
            return await ValidateRequest(request)
                .BindAsync(req => GetCurrentMaritalStateAsync(req.Id, cancellationToken))
                .BindAsync(currentMaritalState =>
                    RemoveMaritalStateAsync(currentMaritalState, cancellationToken)
                )
                .MapAsync(currentMaritalState =>
                {
                    return new Response<MaritalStateResponse>(null);
                });
        }

        private static Result<RemoveMaritalStateRequest> ValidateRequest(
            RemoveMaritalStateRequest request
        )
        {
            return request.Id == default
                ? Result<RemoveMaritalStateRequest>.Failure(MaritalStateErrors.NotFound(request.Id))
                : Result<RemoveMaritalStateRequest>.Success(request);
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

        private async Task<Result<MaritalState>> RemoveMaritalStateAsync(
            MaritalState removeMaritalState,
            CancellationToken cancellationToken
        )
        {
            removeMaritalState.DeletedAt = _dateTimeService.UtcNow;
            removeMaritalState.EditedAt = _dateTimeService.UtcNow;
            removeMaritalState.EditedBy = _authenticatedUserService.UserId;

            removeMaritalState.AddEvent(new MaritalStateRemovedEvent(removeMaritalState.Id));

            await ExecuteTransactionAsync(
                () => _maritalstateDbContext.Update(removeMaritalState),
                removeMaritalState.GetEvents(),
                cancellationToken
            );

            return Result<MaritalState>.Success(removeMaritalState);
        }
    }
}
