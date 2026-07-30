using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Features.Peoples.Skills.Exceptions;
using Services.Features.Peoples.Skills.Models;
using Services.Features.Peoples.Skills.Models.Events;
using Services.Features.Peoples.Skills.Repositories;
using Services.Features.Peoples.Skills.UseCases.Commands;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.DateTime;
using Shared.Domain.Abstractions.Primitives;
using Shared.Domain.Abstractions.Security;

namespace Services.Features.Financials.Skills.UseCases.Commands
{
    public class EditSkillRequestHandler(
        IAuthenticatedUserService authenticatedUserService,
        IDateTimeService dateTimeService,
        IMediator mediator,
        SkillDbContext skillDbContext
    )
        : CommandHandler(skillDbContext, mediator),
            IRequestHandler<EditSkillRequest, Result<Response<SkillResponse>>>
    {
        private readonly SkillDbContext _skillDbContext = skillDbContext;

        private readonly IAuthenticatedUserService _authenticatedUserService =
            authenticatedUserService;

        private readonly IDateTimeService _dateTimeService = dateTimeService;

        public async Task<Result<Response<SkillResponse>>> Handle(
            EditSkillRequest request,
            CancellationToken cancellationToken
        )
        {
            return await ValidateRequest(request)
                .BindAsync(req => GetCurrentSkillAsync(req.Id, cancellationToken))
                .BindAsync(currentSkill =>
                    EditAndSaveSkillAsync(currentSkill, request, cancellationToken)
                )
                .MapAsync(currentSkill =>
                {
                    return new Response<SkillResponse>(null);
                });
        }

        private static Result<EditSkillRequest> ValidateRequest(EditSkillRequest request)
        {
            return request.Id != request.RequestId
                ? Result<EditSkillRequest>.Failure(
                    SkillErrors.PreConditionFailed(request.RequestId)
                )
                : Result<EditSkillRequest>.Success(request);
        }

        private async Task<Result<Skill>> GetCurrentSkillAsync(
            int id,
            CancellationToken cancellationToken
        )
        {
            var skill = await _skillDbContext
                .Skills.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id && !x.DeletedAt.HasValue, cancellationToken);

            return skill is null
                ? Result<Skill>.Failure(SkillErrors.NotFound(id))
                : Result<Skill>.Success(skill);
        }

        private async Task<Result<Skill>> EditAndSaveSkillAsync(
            Skill currentSkill,
            EditSkillRequest request,
            CancellationToken cancellationToken
        )
        {
            var editSkill = new Skill(
                request.Id,
                request.TalentId,
                request.Description,
                request.Institute,
                request.SkillTypeId,
                request.SkillCategoryId,
                request.SkillLevelId,
                request.SkillLevelMaxId,
                request.SkillStateId,
                request.StartDate,
                request.EndDate,
                request.Picture ?? Shared.Infrastructure.Resources.Images.PictureBase64Image,
                currentSkill.CreatedAt.GetValueOrDefault(),
                currentSkill.CreatedBy,
                _dateTimeService.UtcNow,
                _authenticatedUserService.UserId
            );

            editSkill.AddEvent(new SkillEditedEvent(editSkill.Id));

            await ExecuteTransactionAsync(
                () => _skillDbContext.Skills.Update(editSkill),
                editSkill.GetEvents(),
                cancellationToken
            );

            return Result<Skill>.Success(editSkill);
        }
    }
}
