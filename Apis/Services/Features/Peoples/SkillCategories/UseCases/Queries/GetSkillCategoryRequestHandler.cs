using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Features.Peoples.SkillCategories.Exceptions;
using Services.Features.Peoples.SkillCategories.Models;
using Services.Features.Peoples.SkillCategories.Repositories;
using Services.Features.Peoples.SkillCategories.UseCases.Queries;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.Primitives;
using Shared.Infrastructure.Extensions;

namespace Services.Features.Financials.SkillCategorys.UseCases.Queries
{
    public class GetSkillCategoryRequestHandler(
        IMapper mapper,
        IMediator mediator,
        SkillCategoryDbContext skillcategoryDbContext
    )
        : CommandHandler(skillcategoryDbContext, mediator),
            IRequestHandler<
                GetSkillCategoryRequest,
                Result<Response<IEnumerable<SkillCategoryResponse>>>
            >
    {
        private readonly SkillCategoryDbContext _skillcategoryDbContext = skillcategoryDbContext;

        public async Task<Result<Response<IEnumerable<SkillCategoryResponse>>>> Handle(
            GetSkillCategoryRequest request,
            CancellationToken cancellationToken
        )
        {
            return await GetSkillCategoryAsync(request)
                .BindAsync(skillcategorys => Task.FromResult(GenerateResponse(skillcategorys)));
        }

        private async Task<Result<Pagination<SkillCategory>>> GetSkillCategoryAsync(
            GetSkillCategoryRequest request
        )
        {
            var skillcategorys = await Task.Run(
                () =>
                    _skillcategoryDbContext
                        .SkillCategories.AsNoTracking()
                        .Where(x => !x.DeletedAt.HasValue)
                        .SortBy(request.Query.SortBy.ToString(), request.Query.SortOrderAscending)
                        .CreatePagination(request.Query.Offset, request.Query.Limit)
                    ?? new Pagination<SkillCategory>()
            );

            return !skillcategorys.Rows.Any()
                ? Result<Pagination<SkillCategory>>.Failure(SkillCategoryErrors.IsEmpty())
                : Result<Pagination<SkillCategory>>.Success(skillcategorys);
        }

        private Result<Response<IEnumerable<SkillCategoryResponse>>> GenerateResponse(
            Pagination<SkillCategory> paginationSkillCategory
        )
        {
            var skillcategoryResponse = mapper.Map<IEnumerable<SkillCategoryResponse>>(
                paginationSkillCategory.Rows
            );
            var response = new Response<IEnumerable<SkillCategoryResponse>>(
                skillcategoryResponse,
                paginationSkillCategory.Offset,
                paginationSkillCategory.Limit,
                paginationSkillCategory.PageCount,
                paginationSkillCategory.RowCount
            );
            return Result<Response<IEnumerable<SkillCategoryResponse>>>.Success(response);
        }
    }
}
