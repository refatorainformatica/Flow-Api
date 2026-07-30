using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Features.Peoples.SkillLevels.Exceptions;
using Services.Features.Peoples.SkillLevels.Models;
using Services.Features.Peoples.SkillLevels.Repositories;
using Services.Features.Peoples.SkillLevels.UseCases.Queries;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.Primitives;
using Shared.Infrastructure.Extensions;

namespace Services.Features.Financials.SkillLevels.UseCases.Queries
{
    public class GetBySearchSkillLevelRequestHandler(
        IMapper mapper,
        IMediator mediator,
        SkillLevelDbContext skilllevelDbContext
    )
        : CommandHandler(skilllevelDbContext, mediator),
            IRequestHandler<
                GetBySearchSkillLevelRequest,
                Result<Response<IEnumerable<SkillLevelResponse>>>
            >
    {
        private readonly SkillLevelDbContext _skilllevelDbContext = skilllevelDbContext;

        public async Task<Result<Response<IEnumerable<SkillLevelResponse>>>> Handle(
            GetBySearchSkillLevelRequest request,
            CancellationToken cancellationToken
        )
        {
            return await GetBySearchSkillLevelAsync(request)
                .BindAsync(skilllevels => Task.FromResult(GenerateResponse(skilllevels)));
        }

        private async Task<Result<Pagination<SkillLevel>>> GetBySearchSkillLevelAsync(
            GetBySearchSkillLevelRequest request
        )
        {
            var skilllevels = await Task.Run(
                () =>
                    _skilllevelDbContext
                        .SkillLevels.AsNoTracking()
                        .SearchBy(request.Query.SearchText)
                        .Where(x => !x.DeletedAt.HasValue)
                        .SortBy(request.Query.SortBy.ToString(), request.Query.SortOrderAscending)
                        .CreatePagination(request.Query.Offset, request.Query.Limit)
                    ?? new Pagination<SkillLevel>()
            );

            return !skilllevels.Rows.Any()
                ? Result<Pagination<SkillLevel>>.Failure(
                    SkillLevelErrors.NotFound(request.Query.SearchText)
                )
                : Result<Pagination<SkillLevel>>.Success(skilllevels);
        }

        private Result<Response<IEnumerable<SkillLevelResponse>>> GenerateResponse(
            Pagination<SkillLevel> paginationSkillLevel
        )
        {
            var skilllevelResponse = mapper.Map<IEnumerable<SkillLevelResponse>>(
                paginationSkillLevel.Rows
            );
            var response = new Response<IEnumerable<SkillLevelResponse>>(
                skilllevelResponse,
                paginationSkillLevel.Offset,
                paginationSkillLevel.Limit,
                paginationSkillLevel.PageCount,
                paginationSkillLevel.RowCount
            );
            return Result<Response<IEnumerable<SkillLevelResponse>>>.Success(response);
        }
    }
}
