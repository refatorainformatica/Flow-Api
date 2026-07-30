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
    public class EditSkillStateRequestHandler(
        IAuthenticatedUserService authenticatedUserService,
        IDateTimeService dateTimeService,
        IMediator mediator,
        SkillStateDbContext skillstateDbContext
    )
        : CommandHandler(skillstateDbContext, mediator),
            IRequestHandler<EditSkillStateRequest, Result<Response<SkillStateResponse>>>
    {
        private readonly SkillStateDbContext _skillstateDbContext = skillstateDbContext;

        private readonly IAuthenticatedUserService _authenticatedUserService =
            authenticatedUserService;

        private readonly IDateTimeService _dateTimeService = dateTimeService;

        public async Task<Result<Response<SkillStateResponse>>> Handle(
            EditSkillStateRequest request,
            CancellationToken cancellationToken
        )
        {
            return await ValidateRequest(request)
                .BindAsync(req => GetCurrentSkillStateAsync(req.Id, cancellationToken))
                .BindAsync(currentSkillState =>
                    EditAndSaveSkillStateAsync(currentSkillState, request, cancellationToken)
                )
                .MapAsync(currentSkillState =>
                {
                    return new Response<SkillStateResponse>(null);
                });
        }

        private static Result<EditSkillStateRequest> ValidateRequest(EditSkillStateRequest request)
        {
            return request.Id != request.RequestId
                ? Result<EditSkillStateRequest>.Failure(
                    SkillStateErrors.PreConditionFailed(request.RequestId)
                )
                : Result<EditSkillStateRequest>.Success(request);
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

        private async Task<Result<SkillState>> EditAndSaveSkillStateAsync(
            SkillState currentSkillState,
            EditSkillStateRequest request,
            CancellationToken cancellationToken
        )
        {
            var editSkillState = new SkillState(
                request.Id,
                request.Description,
                request.Picture ?? Shared.Infrastructure.Resources.Images.PictureBase64Image,
                currentSkillState.CreatedAt.GetValueOrDefault(),
                currentSkillState.CreatedBy,
                _dateTimeService.UtcNow,
                _authenticatedUserService.UserId
            );

            editSkillState.AddEvent(new SkillStateEditedEvent(editSkillState.Id));

            await ExecuteTransactionAsync(
                () => _skillstateDbContext.SkillStates.Update(editSkillState),
                editSkillState.GetEvents(),
                cancellationToken
            );

            return Result<SkillState>.Success(editSkillState);
        }
    }
}
