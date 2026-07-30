using AutoMapper;
using MediatR;
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
    public class CreateActivityBranchRequestHandler(
        IAuthenticatedUserService authenticatedUserService,
        IDateTimeService dateTimeService,
        IMapper mapper,
        IMediator mediator,
        ActivityBranchDbContext activitybranchDbContext
    )
        : CommandHandler(activitybranchDbContext, mediator),
            IRequestHandler<CreateActivityBranchRequest, Result<Response<ActivityBranchResponse>>>
    {
        private readonly ActivityBranchDbContext _activitybranchDbContext = activitybranchDbContext;

        private readonly IAuthenticatedUserService _authenticatedUserService =
            authenticatedUserService;

        private readonly IDateTimeService _dateTimeService = dateTimeService;

        public async Task<Result<Response<ActivityBranchResponse>>> Handle(
            CreateActivityBranchRequest request,
            CancellationToken cancellationToken
        )
        {
            return await SaveActivityBranchAsync(request, cancellationToken)
                .BindAsync(activitybranch => Task.FromResult(GenerateResponse(activitybranch)));
        }

        private async Task<Result<ActivityBranch>> SaveActivityBranchAsync(
            CreateActivityBranchRequest request,
            CancellationToken cancellationToken
        )
        {
            var newActivityBranch = new ActivityBranch(
                0,
                request.ExternalCode,
                request.Description,
                request.Picture ?? Shared.Infrastructure.Resources.Images.PictureBase64Image,
                _dateTimeService.UtcNow,
                _authenticatedUserService.UserId,
                _dateTimeService.UtcNow,
                _authenticatedUserService.UserId
            );

            newActivityBranch.AddEvent(new ActivityBranchCreatedEvent(newActivityBranch.Id));

            await ExecuteTransactionAsync(
                async () =>
                {
                    await _activitybranchDbContext.ActivityBranchs.AddAsync(
                        newActivityBranch,
                        cancellationToken: cancellationToken
                    );
                },
                newActivityBranch.GetEvents(),
                cancellationToken: cancellationToken
            );

            return Result<ActivityBranch>.Success(newActivityBranch);
        }

        private Result<Response<ActivityBranchResponse>> GenerateResponse(
            ActivityBranch activitybranch
        )
        {
            var activitybranchResponse = mapper.Map<ActivityBranchResponse>(activitybranch);
            var response = new Response<ActivityBranchResponse>(activitybranchResponse);

            return Result<Response<ActivityBranchResponse>>.Success(response);
        }
    }
}
