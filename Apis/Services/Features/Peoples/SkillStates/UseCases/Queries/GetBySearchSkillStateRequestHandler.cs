using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Features.Peoples.SkillStates.Exceptions;
using Services.Features.Peoples.SkillStates.Models;
using Services.Features.Peoples.SkillStates.Repositories;
using Services.Features.Peoples.SkillStates.UseCases.Queries;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.Primitives;
using Shared.Infrastructure.Extensions;

namespace Services.Features.Financials.SkillStates.UseCases.Queries
{
    public class GetBySearchSkillStateRequestHandler(
        IMapper mapper,
        IMediator mediator,
        SkillStateDbContext skillstateDbContext
    )
        : CommandHandler(skillstateDbContext, mediator),
            IRequestHandler<
                GetBySearchSkillStateRequest,
                Result<Response<IEnumerable<SkillStateResponse>>>
            >
    {
        private readonly SkillStateDbContext _skillstateDbContext = skillstateDbContext;

        public async Task<Result<Response<IEnumerable<SkillStateResponse>>>> Handle(
            GetBySearchSkillStateRequest request,
            CancellationToken cancellationToken
        )
        {
            return await GetBySearchSkillStateAsync(request)
                .BindAsync(skillstates => Task.FromResult(GenerateResponse(skillstates)));
        }

        private async Task<Result<Pagination<SkillState>>> GetBySearchSkillStateAsync(
            GetBySearchSkillStateRequest request
        )
        {
            var skillstates = await Task.Run(
                () =>
                    _skillstateDbContext
                        .SkillStates.AsNoTracking()
                        .SearchBy(request.Query.SearchText)
                        .Where(x => !x.DeletedAt.HasValue)
                        .SortBy(request.Query.SortBy.ToString(), request.Query.SortOrderAscending)
                        .CreatePagination(request.Query.Offset, request.Query.Limit)
                    ?? new Pagination<SkillState>()
            );

            return !skillstates.Rows.Any()
                ? Result<Pagination<SkillState>>.Failure(
                    SkillStateErrors.NotFound(request.Query.SearchText)
                )
                : Result<Pagination<SkillState>>.Success(skillstates);
        }

        private Result<Response<IEnumerable<SkillStateResponse>>> GenerateResponse(
            Pagination<SkillState> paginationSkillState
        )
        {
            var skillstateResponse = mapper.Map<IEnumerable<SkillStateResponse>>(
                paginationSkillState.Rows
            );
            var response = new Response<IEnumerable<SkillStateResponse>>(
                skillstateResponse,
                paginationSkillState.Offset,
                paginationSkillState.Limit,
                paginationSkillState.PageCount,
                paginationSkillState.RowCount
            );
            return Result<Response<IEnumerable<SkillStateResponse>>>.Success(response);
        }
    }
}
