using AutoMapper;
using MediatR;
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
    public class CreateSkillRequestHandler(
        IAuthenticatedUserService authenticatedUserService,
        IDateTimeService dateTimeService,
        IMapper mapper,
        IMediator mediator,
        SkillDbContext skillDbContext
    )
        : CommandHandler(skillDbContext, mediator),
            IRequestHandler<CreateSkillRequest, Result<Response<SkillResponse>>>
    {
        private readonly SkillDbContext _skillDbContext = skillDbContext;

        private readonly IAuthenticatedUserService _authenticatedUserService =
            authenticatedUserService;

        private readonly IDateTimeService _dateTimeService = dateTimeService;

        public async Task<Result<Response<SkillResponse>>> Handle(
            CreateSkillRequest request,
            CancellationToken cancellationToken
        )
        {
            return await SaveSkillAsync(request, cancellationToken)
                .BindAsync(skill => Task.FromResult(GenerateResponse(skill)));
        }

        private async Task<Result<Skill>> SaveSkillAsync(
            CreateSkillRequest request,
            CancellationToken cancellationToken
        )
        {
            var newSkill = new Skill(
                0,
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
                _dateTimeService.UtcNow,
                _authenticatedUserService.UserId,
                _dateTimeService.UtcNow,
                _authenticatedUserService.UserId
            );

            newSkill.AddEvent(new SkillCreatedEvent(newSkill.Id));

            await ExecuteTransactionAsync(
                async () =>
                {
                    await _skillDbContext.Skills.AddAsync(
                        newSkill,
                        cancellationToken: cancellationToken
                    );
                },
                newSkill.GetEvents(),
                cancellationToken: cancellationToken
            );

            return Result<Skill>.Success(newSkill);
        }

        private Result<Response<SkillResponse>> GenerateResponse(Skill skill)
        {
            var skillResponse = mapper.Map<SkillResponse>(skill);
            var response = new Response<SkillResponse>(skillResponse);

            return Result<Response<SkillResponse>>.Success(response);
        }
    }
}
