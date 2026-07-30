using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Features.Peoples.SkillStates.Exceptions;
using Services.Features.Peoples.SkillStates.Models;
using Services.Features.Peoples.SkillStates.Models.Events;
using Services.Features.Peoples.SkillStates.Repositories;
using Services.Features.Peoples.SkillStates.UseCases.Commands;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.DateTime;
using Shared.Domain.Abstractions.Primitives;
using Shared.Domain.Abstractions.Security;

namespace Services.Features.Financials.SkillStates.UseCases.Commands
{
    public class RemoveSkillStateRequestHandler(
        IAuthenticatedUserService authenticatedUserService,
        IDateTimeService dateTimeService,
        IMediator mediator,
        SkillStateDbContext skillstateDbContext
    )
        : CommandHandler(skillstateDbContext, mediator),
            IRequestHandler<RemoveSkillStateRequest, Result<Response<SkillStateResponse>>>
    {
        private readonly SkillStateDbContext _skillstateDbContext = skillstateDbContext;

        private readonly IAuthenticatedUserService _authenticatedUserService =
            authenticatedUserService;

        private readonly IDateTimeService _dateTimeService = dateTimeService;

        public async Task<Result<Response<SkillStateResponse>>> Handle(
            RemoveSkillStateRequest request,
            CancellationToken cancellationToken
        )
        {
            return await ValidateRequest(request)
                .BindAsync(req => GetCurrentSkillStateAsync(req.Id, cancellationToken))
                .BindAsync(currentSkillState =>
                    RemoveSkillStateAsync(currentSkillState, cancellationToken)
                )
                .MapAsync(currentSkillState =>
                {
                    return new Response<SkillStateResponse>(null);
                });
        }

        private static Result<RemoveSkillStateRequest> ValidateRequest(
            RemoveSkillStateRequest request
        )
        {
            return request.Id == default
                ? Result<RemoveSkillStateRequest>.Failure(SkillStateErrors.NotFound(request.Id))
                : Result<RemoveSkillStateRequest>.Success(request);
        }

        private async Task<Result<SkillState>> GetCurrentSkillStateAsync(
            int id,
            CancellationToken cancellationToken
        )
        {
            var skillstate = await _skillstateDbContext
                .SkillStates.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id && !x.DeletedAt.HasValue, cancellationToken);

            return skillstate is null
                ? Result<SkillState>.Failure(SkillStateErrors.NotFound(id))
                : Result<SkillState>.Success(skillstate);
        }

        private async Task<Result<SkillState>> RemoveSkillStateAsync(
            SkillState removeSkillState,
            CancellationToken cancellationToken
        )
        {
            removeSkillState.DeletedAt = _dateTimeService.UtcNow;
            removeSkillState.EditedAt = _dateTimeService.UtcNow;
            removeSkillState.EditedBy = _authenticatedUserService.UserId;

            removeSkillState.AddEvent(new SkillStateRemovedEvent(removeSkillState.Id));

            await ExecuteTransactionAsync(
                () => _skillstateDbContext.Update(removeSkillState),
                removeSkillState.GetEvents(),
                cancellationToken
            );

            return Result<SkillState>.Success(removeSkillState);
        }
    }
}
