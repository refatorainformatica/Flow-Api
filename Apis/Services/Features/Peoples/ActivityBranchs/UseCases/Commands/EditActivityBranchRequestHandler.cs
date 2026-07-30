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
    public class EditActivityBranchRequestHandler(
        IAuthenticatedUserService authenticatedUserService,
        IDateTimeService dateTimeService,
        IMediator mediator,
        ActivityBranchDbContext activitybranchDbContext
    )
        : CommandHandler(activitybranchDbContext, mediator),
            IRequestHandler<EditActivityBranchRequest, Result<Response<ActivityBranchResponse>>>
    {
        private readonly ActivityBranchDbContext _activitybranchDbContext = activitybranchDbContext;

        private readonly IAuthenticatedUserService _authenticatedUserService =
            authenticatedUserService;

        private readonly IDateTimeService _dateTimeService = dateTimeService;

        public async Task<Result<Response<ActivityBranchResponse>>> Handle(
            EditActivityBranchRequest request,
            CancellationToken cancellationToken
        )
        {
            return await ValidateRequest(request)
                .BindAsync(req => GetCurrentActivityBranchAsync(req.Id, cancellationToken))
                .BindAsync(currentActivityBranch =>
                    EditAndSaveActivityBranchAsync(
                        currentActivityBranch,
                        request,
                        cancellationToken
                    )
                )
                .MapAsync(currentActivityBranch =>
                {
                    return new Response<ActivityBranchResponse>(null);
                });
        }

        private static Result<EditActivityBranchRequest> ValidateRequest(
            EditActivityBranchRequest request
        )
        {
            return request.Id != request.RequestId
                ? Result<EditActivityBranchRequest>.Failure(
                    ActivityBranchErrors.PreConditionFailed(request.RequestId)
                )
                : Result<EditActivityBranchRequest>.Success(request);
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

        private async Task<Result<ActivityBranch>> EditAndSaveActivityBranchAsync(
            ActivityBranch currentActivityBranch,
            EditActivityBranchRequest request,
            CancellationToken cancellationToken
        )
        {
            var editActivityBranch = new ActivityBranch(
                request.Id,
                request.ExternalCode,
                request.Description,
                request.Picture ?? Shared.Infrastructure.Resources.Images.PictureBase64Image,
                currentActivityBranch.CreatedAt.GetValueOrDefault(),
                currentActivityBranch.CreatedBy,
                _dateTimeService.UtcNow,
                _authenticatedUserService.UserId
            );

            editActivityBranch.AddEvent(new ActivityBranchEditedEvent(editActivityBranch.Id));

            await ExecuteTransactionAsync(
                () => _activitybranchDbContext.ActivityBranchs.Update(editActivityBranch),
                editActivityBranch.GetEvents(),
                cancellationToken
            );

            return Result<ActivityBranch>.Success(editActivityBranch);
        }
    }
}
