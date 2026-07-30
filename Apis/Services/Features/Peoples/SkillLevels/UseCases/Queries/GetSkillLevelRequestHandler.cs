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
    public class GetSkillLevelRequestHandler(
        IMapper mapper,
        IMediator mediator,
        SkillLevelDbContext skilllevelDbContext
    )
        : CommandHandler(skilllevelDbContext, mediator),
            IRequestHandler<GetSkillLevelRequest, Result<Response<IEnumerable<SkillLevelResponse>>>>
    {
        private readonly SkillLevelDbContext _skilllevelDbContext = skilllevelDbContext;

        public async Task<Result<Response<IEnumerable<SkillLevelResponse>>>> Handle(
            GetSkillLevelRequest request,
            CancellationToken cancellationToken
        )
        {
            return await GetSkillLevelAsync(request)
                .BindAsync(skilllevels => Task.FromResult(GenerateResponse(skilllevels)));
        }

        private async Task<Result<Pagination<SkillLevel>>> GetSkillLevelAsync(
            GetSkillLevelRequest request
        )
        {
            var skilllevels = await Task.Run(
                () =>
                    _skilllevelDbContext
                        .SkillLevels.AsNoTracking()
                        .Where(x => !x.DeletedAt.HasValue)
                        .SortBy(request.Query.SortBy.ToString(), request.Query.SortOrderAscending)
                        .CreatePagination(request.Query.Offset, request.Query.Limit)
                    ?? new Pagination<SkillLevel>()
            );

            return !skilllevels.Rows.Any()
                ? Result<Pagination<SkillLevel>>.Failure(SkillLevelErrors.IsEmpty())
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
