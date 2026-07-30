using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Features.Peoples.Skills.Exceptions;
using Services.Features.Peoples.Skills.Models;
using Services.Features.Peoples.Skills.Repositories;
using Services.Features.Peoples.Skills.UseCases.Queries;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.Primitives;
using Shared.Infrastructure.Extensions;

namespace Services.Features.Financials.Skills.UseCases.Queries
{
    public class GetSkillRequestHandler(
        IMapper mapper,
        IMediator mediator,
        SkillDbContext skillDbContext
    )
        : CommandHandler(skillDbContext, mediator),
            IRequestHandler<GetSkillRequest, Result<Response<IEnumerable<SkillResponse>>>>
    {
        private readonly SkillDbContext _skillDbContext = skillDbContext;

        public async Task<Result<Response<IEnumerable<SkillResponse>>>> Handle(
            GetSkillRequest request,
            CancellationToken cancellationToken
        )
        {
            return await GetSkillAsync(request)
                .BindAsync(skills => Task.FromResult(GenerateResponse(skills)));
        }

        private async Task<Result<Pagination<Skill>>> GetSkillAsync(GetSkillRequest request)
        {
            var skills = await Task.Run(
                () =>
                    _skillDbContext
                        .Skills.AsNoTracking()
                        .Where(x => !x.DeletedAt.HasValue)
                        .SortBy(request.Query.SortBy.ToString(), request.Query.SortOrderAscending)
                        .CreatePagination(request.Query.Offset, request.Query.Limit)
                    ?? new Pagination<Skill>()
            );

            return !skills.Rows.Any()
                ? Result<Pagination<Skill>>.Failure(SkillErrors.IsEmpty())
                : Result<Pagination<Skill>>.Success(skills);
        }

        private Result<Response<IEnumerable<SkillResponse>>> GenerateResponse(
            Pagination<Skill> paginationSkill
        )
        {
            var skillResponse = mapper.Map<IEnumerable<SkillResponse>>(paginationSkill.Rows);
            var response = new Response<IEnumerable<SkillResponse>>(
                skillResponse,
                paginationSkill.Offset,
                paginationSkill.Limit,
                paginationSkill.PageCount,
                paginationSkill.RowCount
            );
            return Result<Response<IEnumerable<SkillResponse>>>.Success(response);
        }
    }
}
