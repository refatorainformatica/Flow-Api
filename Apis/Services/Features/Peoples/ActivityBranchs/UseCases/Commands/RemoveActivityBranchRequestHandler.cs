using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Features.Peoples.ActivityBranchs.Exceptions;
using Services.Features.Peoples.ActivityBranchs.Models;
using Services.Features.Peoples.ActivityBranchs.Models.Events;
using Services.Features.Peoples.ActivityBranchs.Repositories;
using Services.Features.Peoples.ActivityBranchs.UseCases.Commands;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.DateTime;
using Shared.Domain.Abstractions.Primitives;
using Shared.Domain.Abstractions.Security;

namespace Services.Features.Financials.ActivityBranchs.UseCases.Commands
{
    public class RemoveActivityBranchRequestHandler(
        IAuthenticatedUserService authenticatedUserService,
        IDateTimeService dateTimeService,
        IMediator mediator,
        ActivityBranchDbContext activitybranchDbContext
    )
        : CommandHandler(activitybranchDbContext, mediator),
            IRequestHandler<RemoveActivityBranchRequest, Result<Response<ActivityBranchResponse>>>
    {
        private readonly ActivityBranchDbContext _activitybranchDbContext = activitybranchDbContext;

        private readonly IAuthenticatedUserService _authenticatedUserService =
            authenticatedUserService;

        private readonly IDateTimeService _dateTimeService = dateTimeService;

        public async Task<Result<Response<ActivityBranchResponse>>> Handle(
            RemoveActivityBranchRequest request,
            CancellationToken cancellationToken
        )
        {
            return await ValidateRequest(request)
                .BindAsync(req => GetCurrentActivityBranchAsync(req.Id, cancellationToken))
                .BindAsync(currentActivityBranch =>
                    RemoveActivityBranchAsync(currentActivityBranch, cancellationToken)
                )
                .MapAsync(currentActivityBranch =>
                {
                    return new Response<ActivityBranchResponse>(null);
                });
        }

        private static Result<RemoveActivityBranchRequest> ValidateRequest(
            RemoveActivityBranchRequest request
        )
        {
            return request.Id == default
                ? Result<RemoveActivityBranchRequest>.Failure(
                    ActivityBranchErrors.NotFound(request.Id)
                )
                : Result<RemoveActivityBranchRequest>.Success(request);
        }

        private async Task<Result<ActivityBranch>> GetCurrentActivityBranchAsync(
            int id,
            CancellationToken cancellationToken
        )
        {
            var activitybranch = await _activitybranchDbContext
                .ActivityBranchs.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id && !x.DeletedAt.HasValue, cancellationToken);

            return activitybranch is null
                ? Result<ActivityBranch>.Failure(ActivityBranchErrors.NotFound(id))
                : Result<ActivityBranch>.Success(activitybranch);
        }

        private async Task<Result<ActivityBranch>> RemoveActivityBranchAsync(
            ActivityBranch removeActivityBranch,
            CancellationToken cancellationToken
        )
        {
            removeActivityBranch.DeletedAt = _dateTimeService.UtcNow;
            removeActivityBranch.EditedAt = _dateTimeService.UtcNow;
            removeActivityBranch.EditedBy = _authenticatedUserService.UserId;

            removeActivityBranch.AddEvent(new ActivityBranchRemovedEvent(removeActivityBranch.Id));

            await ExecuteTransactionAsync(
                () => _activitybranchDbContext.Update(removeActivityBranch),
                removeActivityBranch.GetEvents(),
                cancellationToken
            );

            return Result<ActivityBranch>.Success(removeActivityBranch);
        }
    }
}
