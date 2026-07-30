using AutoMapper;
using MediatR;
using Services.Features.Peoples.SkillCategories.Models;
using Services.Features.Peoples.SkillCategories.Models.Events;
using Services.Features.Peoples.SkillCategories.Repositories;
using Services.Features.Peoples.SkillCategories.UseCases.Commands;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.DateTime;
using Shared.Domain.Abstractions.Primitives;
using Shared.Domain.Abstractions.Security;

namespace Services.Features.Financials.SkillCategorys.UseCases.Commands
{
    public class CreateSkillCategoryRequestHandler(
        IAuthenticatedUserService authenticatedUserService,
        IDateTimeService dateTimeService,
        IMapper mapper,
        IMediator mediator,
        SkillCategoryDbContext skillcategoryDbContext
    )
        : CommandHandler(skillcategoryDbContext, mediator),
            IRequestHandler<CreateSkillCategoryRequest, Result<Response<SkillCategoryResponse>>>
    {
        private readonly SkillCategoryDbContext _skillcategoryDbContext = skillcategoryDbContext;

        private readonly IAuthenticatedUserService _authenticatedUserService =
            authenticatedUserService;

        private readonly IDateTimeService _dateTimeService = dateTimeService;

        public async Task<Result<Response<SkillCategoryResponse>>> Handle(
            CreateSkillCategoryRequest request,
            CancellationToken cancellationToken
        )
        {
            return await SaveSkillCategoryAsync(request, cancellationToken)
                .BindAsync(skillcategory => Task.FromResult(GenerateResponse(skillcategory)));
        }

        private async Task<Result<SkillCategory>> SaveSkillCategoryAsync(
            CreateSkillCategoryRequest request,
            CancellationToken cancellationToken
        )
        {
            var newSkillCategory = new SkillCategory(
                0,
                request.Description,
                request.Picture ?? Shared.Infrastructure.Resources.Images.PictureBase64Image,
                _dateTimeService.UtcNow,
                _authenticatedUserService.UserId,
                _dateTimeService.UtcNow,
                _authenticatedUserService.UserId
            );

            newSkillCategory.AddEvent(new SkillCategoryCreatedEvent(newSkillCategory.Id));

            await ExecuteTransactionAsync(
                async () =>
                {
                    await _skillcategoryDbContext.SkillCategories.AddAsync(
                        newSkillCategory,
                        cancellationToken: cancellationToken
                    );
                },
                newSkillCategory.GetEvents(),
                cancellationToken: cancellationToken
            );

            return Result<SkillCategory>.Success(newSkillCategory);
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
