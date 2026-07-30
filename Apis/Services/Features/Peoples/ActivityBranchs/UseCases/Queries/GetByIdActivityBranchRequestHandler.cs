using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Features.Peoples.ActivityBranchs.Exceptions;
using Services.Features.Peoples.ActivityBranchs.Models;
using Services.Features.Peoples.ActivityBranchs.Repositories;
using Services.Features.Peoples.ActivityBranchs.UseCases.Queries;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.ActivityBranchs.UseCases.Queries
{
    public class GetByIdActivityBranchRequestHandler(
        IMapper mapper,
        IMediator mediator,
        ActivityBranchDbContext activitybranchDbContext
    )
        : CommandHandler(activitybranchDbContext, mediator),
            IRequestHandler<GetByIdActivityBranchRequest, Result<Response<ActivityBranchResponse>>>
    {
        private readonly ActivityBranchDbContext _activitybranchDbContext = activitybranchDbContext;

        public async Task<Result<Response<ActivityBranchResponse>>> Handle(
            GetByIdActivityBranchRequest request,
            CancellationToken cancellationToken
        )
        {
            return await GetByIdActivityBranchAsync(request, cancellationToken)
                .BindAsync(activitybranchs => Task.FromResult(GenerateResponse(activitybranchs)));
        }

        private async Task<Result<ActivityBranch>> GetByIdActivityBranchAsync(
            GetByIdActivityBranchRequest request,
            CancellationToken cancellationToken
        )
        {
            var activitybranch = await _activitybranchDbContext
                .ActivityBranchs.AsNoTracking()
                .FirstOrDefaultAsync(
                    x => x.Id == request.Id && !x.DeletedAt.HasValue,
                    cancellationToken
                );

            return activitybranch is null
                ? Result<ActivityBranch>.Failure(ActivityBranchErrors.NotFound(request.Id))
                : Result<ActivityBranch>.Success(activitybranch);
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
