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
    public class RemoveSkillCategoryRequestHandler(
        IAuthenticatedUserService authenticatedUserService,
        IDateTimeService dateTimeService,
        IMediator mediator,
        SkillCategoryDbContext skillcategoryDbContext
    )
        : CommandHandler(skillcategoryDbContext, mediator),
            IRequestHandler<RemoveSkillCategoryRequest, Result<Response<SkillCategoryResponse>>>
    {
        private readonly SkillCategoryDbContext _skillcategoryDbContext = skillcategoryDbContext;

        private readonly IAuthenticatedUserService _authenticatedUserService =
            authenticatedUserService;

        private readonly IDateTimeService _dateTimeService = dateTimeService;

        public async Task<Result<Response<SkillCategoryResponse>>> Handle(
            RemoveSkillCategoryRequest request,
            CancellationToken cancellationToken
        )
        {
            return await ValidateRequest(request)
                .BindAsync(req => GetCurrentSkillCategoryAsync(req.Id, cancellationToken))
                .BindAsync(currentSkillCategory =>
                    RemoveSkillCategoryAsync(currentSkillCategory, cancellationToken)
                )
                .MapAsync(currentSkillCategory =>
                {
                    return new Response<SkillCategoryResponse>(null);
                });
        }

        private static Result<RemoveSkillCategoryRequest> ValidateRequest(
            RemoveSkillCategoryRequest request
        )
        {
            return request.Id == default
                ? Result<RemoveSkillCategoryRequest>.Failure(
                    SkillCategoryErrors.NotFound(request.Id)
                )
                : Result<RemoveSkillCategoryRequest>.Success(request);
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

        private async Task<Result<SkillCategory>> RemoveSkillCategoryAsync(
            SkillCategory removeSkillCategory,
            CancellationToken cancellationToken
        )
        {
            removeSkillCategory.DeletedAt = _dateTimeService.UtcNow;
            removeSkillCategory.EditedAt = _dateTimeService.UtcNow;
            removeSkillCategory.EditedBy = _authenticatedUserService.UserId;

            removeSkillCategory.AddEvent(new SkillCategoryRemovedEvent(removeSkillCategory.Id));

            await ExecuteTransactionAsync(
                () => _skillcategoryDbContext.Update(removeSkillCategory),
                removeSkillCategory.GetEvents(),
                cancellationToken
            );

            return Result<SkillCategory>.Success(removeSkillCategory);
        }
    }
}
