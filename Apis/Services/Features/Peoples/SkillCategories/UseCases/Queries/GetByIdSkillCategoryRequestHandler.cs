using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Features.Peoples.SkillCategories.Exceptions;
using Services.Features.Peoples.SkillCategories.Models;
using Services.Features.Peoples.SkillCategories.Repositories;
using Services.Features.Peoples.SkillCategories.UseCases.Queries;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.SkillCategorys.UseCases.Queries
{
    public class GetByIdSkillCategoryRequestHandler(
        IMapper mapper,
        IMediator mediator,
        SkillCategoryDbContext skillcategoryDbContext
    )
        : CommandHandler(skillcategoryDbContext, mediator),
            IRequestHandler<GetByIdSkillCategoryRequest, Result<Response<SkillCategoryResponse>>>
    {
        private readonly SkillCategoryDbContext _skillcategoryDbContext = skillcategoryDbContext;

        public async Task<Result<Response<SkillCategoryResponse>>> Handle(
            GetByIdSkillCategoryRequest request,
            CancellationToken cancellationToken
        )
        {
            return await GetByIdSkillCategoryAsync(request, cancellationToken)
                .BindAsync(skillcategorys => Task.FromResult(GenerateResponse(skillcategorys)));
        }

        private async Task<Result<SkillCategory>> GetByIdSkillCategoryAsync(
            GetByIdSkillCategoryRequest request,
            CancellationToken cancellationToken
        )
        {
            var skillcategory = await _skillcategoryDbContext
                .SkillCategories.AsNoTracking()
                .FirstOrDefaultAsync(
                    x => x.Id == request.Id && !x.DeletedAt.HasValue,
                    cancellationToken
                );

            return skillcategory is null
                ? Result<SkillCategory>.Failure(SkillCategoryErrors.NotFound(request.Id))
                : Result<SkillCategory>.Success(skillcategory);
        }

        private Result<Response<SkillCategoryResponse>> GenerateResponse(
            SkillCategory skillcategory
        )
        {
            var skillcategoryResponse = mapper.Map<SkillCategoryResponse>(skillcategory);
            var response = new Response<SkillCategoryResponse>(skillcategoryResponse);
            return Result<Response<SkillCategoryResponse>>.Success(response);
        }
    }
}
