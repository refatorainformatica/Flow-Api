using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Features.Peoples.SkillCategories.Exceptions;
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
    public class EditSkillCategoryRequestHandler(
        IAuthenticatedUserService authenticatedUserService,
        IDateTimeService dateTimeService,
        IMediator mediator,
        SkillCategoryDbContext skillcategoryDbContext
    )
        : CommandHandler(skillcategoryDbContext, mediator),
            IRequestHandler<EditSkillCategoryRequest, Result<Response<SkillCategoryResponse>>>
    {
        private readonly SkillCategoryDbContext _skillcategoryDbContext = skillcategoryDbContext;

        private readonly IAuthenticatedUserService _authenticatedUserService =
            authenticatedUserService;

        private readonly IDateTimeService _dateTimeService = dateTimeService;

        public async Task<Result<Response<SkillCategoryResponse>>> Handle(
            EditSkillCategoryRequest request,
            CancellationToken cancellationToken
        )
        {
            return await ValidateRequest(request)
                .BindAsync(req => GetCurrentSkillCategoryAsync(req.Id, cancellationToken))
                .BindAsync(currentSkillCategory =>
                    EditAndSaveSkillCategoryAsync(currentSkillCategory, request, cancellationToken)
                )
                .MapAsync(currentSkillCategory =>
                {
                    return new Response<SkillCategoryResponse>(null);
                });
        }

        private static Result<EditSkillCategoryRequest> ValidateRequest(
            EditSkillCategoryRequest request
        )
        {
            return request.Id != request.RequestId
                ? Result<EditSkillCategoryRequest>.Failure(
                    SkillCategoryErrors.PreConditionFailed(request.RequestId)
                )
                : Result<EditSkillCategoryRequest>.Success(request);
        }

        private async Task<Result<SkillCategory>> GetCurrentSkillCategoryAsync(
            int id,
            CancellationToken cancellationToken
        )
        {
            var skillcategory = await _skillcategoryDbContext
                .SkillCategories.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id && !x.DeletedAt.HasValue, cancellationToken);

            return skillcategory is null
                ? Result<SkillCategory>.Failure(SkillCategoryErrors.NotFound(id))
                : Result<SkillCategory>.Success(skillcategory);
        }

        private async Task<Result<SkillCategory>> EditAndSaveSkillCategoryAsync(
            SkillCategory currentSkillCategory,
            EditSkillCategoryRequest request,
            CancellationToken cancellationToken
        )
        {
            var editSkillCategory = new SkillCategory(
                request.Id,
                request.Description,
                request.Picture ?? Shared.Infrastructure.Resources.Images.PictureBase64Image,
                currentSkillCategory.CreatedAt.GetValueOrDefault(),
                currentSkillCategory.CreatedBy,
                _dateTimeService.UtcNow,
                _authenticatedUserService.UserId
            );

            editSkillCategory.AddEvent(new SkillCategoryEditedEvent(editSkillCategory.Id));

            await ExecuteTransactionAsync(
                () => _skillcategoryDbContext.SkillCategories.Update(editSkillCategory),
                editSkillCategory.GetEvents(),
                cancellationToken
            );

            return Result<SkillCategory>.Success(editSkillCategory);
        }
    }
}
