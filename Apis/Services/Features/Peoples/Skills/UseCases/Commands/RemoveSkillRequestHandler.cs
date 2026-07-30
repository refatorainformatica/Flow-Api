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
    public class RemoveSkillRequestHandler(
        IAuthenticatedUserService authenticatedUserService,
        IDateTimeService dateTimeService,
        IMediator mediator,
        SkillDbContext skillDbContext
    )
        : CommandHandler(skillDbContext, mediator),
            IRequestHandler<RemoveSkillRequest, Result<Response<SkillResponse>>>
    {
        private readonly SkillDbContext _skillDbContext = skillDbContext;

        private readonly IAuthenticatedUserService _authenticatedUserService =
            authenticatedUserService;

        private readonly IDateTimeService _dateTimeService = dateTimeService;

        public async Task<Result<Response<SkillResponse>>> Handle(
            RemoveSkillRequest request,
            CancellationToken cancellationToken
        )
        {
            return await ValidateRequest(request)
                .BindAsync(req => GetCurrentSkillAsync(req.Id, cancellationToken))
                .BindAsync(currentSkill => RemoveSkillAsync(currentSkill, cancellationToken))
                .MapAsync(currentSkill =>
                {
                    return new Response<SkillResponse>(null);
                });
        }

        private static Result<RemoveSkillRequest> ValidateRequest(RemoveSkillRequest request)
        {
            return request.Id == default
                ? Result<RemoveSkillRequest>.Failure(SkillErrors.NotFound(request.Id))
                : Result<RemoveSkillRequest>.Success(request);
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

        private async Task<Result<Skill>> RemoveSkillAsync(
            Skill removeSkill,
            CancellationToken cancellationToken
        )
        {
            removeSkill.DeletedAt = _dateTimeService.UtcNow;
            removeSkill.EditedAt = _dateTimeService.UtcNow;
            removeSkill.EditedBy = _authenticatedUserService.UserId;

            removeSkill.AddEvent(new SkillRemovedEvent(removeSkill.Id));

            await ExecuteTransactionAsync(
                () => _skillDbContext.Update(removeSkill),
                removeSkill.GetEvents(),
                cancellationToken
            );

            return Result<Skill>.Success(removeSkill);
        }
    }
}
