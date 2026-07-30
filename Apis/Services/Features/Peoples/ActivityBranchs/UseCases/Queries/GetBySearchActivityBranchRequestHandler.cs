using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Features.Peoples.ActivityBranchs.Exceptions;
using Services.Features.Peoples.ActivityBranchs.Models;
using Services.Features.Peoples.ActivityBranchs.Repositories;
using Services.Features.Peoples.ActivityBranchs.UseCases.Queries;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.Primitives;
using Shared.Infrastructure.Extensions;

namespace Services.Features.Financials.ActivityBranchs.UseCases.Queries
{
    public class GetBySearchActivityBranchRequestHandler(
        IMapper mapper,
        IMediator mediator,
        ActivityBranchDbContext activitybranchDbContext
    )
        : CommandHandler(activitybranchDbContext, mediator),
            IRequestHandler<
                GetBySearchActivityBranchRequest,
                Result<Response<IEnumerable<ActivityBranchResponse>>>
            >
    {
        private readonly ActivityBranchDbContext _activitybranchDbContext = activitybranchDbContext;

        public async Task<Result<Response<IEnumerable<ActivityBranchResponse>>>> Handle(
            GetBySearchActivityBranchRequest request,
            CancellationToken cancellationToken
        )
        {
            return await GetBySearchActivityBranchAsync(request)
                .BindAsync(activitybranchs => Task.FromResult(GenerateResponse(activitybranchs)));
        }

        private async Task<Result<Pagination<ActivityBranch>>> GetBySearchActivityBranchAsync(
            GetBySearchActivityBranchRequest request
        )
        {
            var activitybranchs = await Task.Run(
                () =>
                    _activitybranchDbContext
                        .ActivityBranchs.AsNoTracking()
                        .SearchBy(request.Query.SearchText)
                        .SortBy(request.Query.SortBy.ToString(), request.Query.SortOrderAscending)
                        .CreatePagination(request.Query.Offset, request.Query.Limit)
                    ?? new Pagination<ActivityBranch>()
            );

            return !activitybranchs.Rows.Any()
                ? Result<Pagination<ActivityBranch>>.Failure(
                    ActivityBranchErrors.NotFound(request.Query.SearchText)
                )
                : Result<Pagination<ActivityBranch>>.Success(activitybranchs);
        }

        private Result<Response<IEnumerable<ActivityBranchResponse>>> GenerateResponse(
            Pagination<ActivityBranch> paginationActivityBranch
        )
        {
            var activitybranchResponse = mapper.Map<IEnumerable<ActivityBranchResponse>>(
                paginationActivityBranch.Rows
            );
            var response = new Response<IEnumerable<ActivityBranchResponse>>(
                activitybranchResponse,
                paginationActivityBranch.Offset,
                paginationActivityBranch.Limit,
                paginationActivityBranch.PageCount,
                paginationActivityBranch.RowCount
            );
            return Result<Response<IEnumerable<ActivityBranchResponse>>>.Success(response);
        }
    }
}
