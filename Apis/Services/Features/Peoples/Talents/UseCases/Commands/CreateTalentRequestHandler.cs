using AutoMapper;
using MediatR;
using Services.Features.Peoples.Skills.Models;
using Services.Features.Peoples.Talents.Models;
using Services.Features.Peoples.Talents.Models.Events;
using Services.Features.Peoples.Talents.Repositories;
using Services.Features.Peoples.Talents.UseCases.Commands;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.DateTime;
using Shared.Domain.Abstractions.Primitives;
using Shared.Domain.Abstractions.Security;

namespace Services.Features.Financials.Talents.UseCases.Commands
{
    public class CreateTalentRequestHandler(
        IAuthenticatedUserService authenticatedUserService,
        IDateTimeService dateTimeService,
        IMapper mapper,
        IMediator mediator,
        TalentDbContext talentDbContext
    )
        : CommandHandler(talentDbContext, mediator),
            IRequestHandler<CreateTalentRequest, Result<Response<TalentResponse>>>
    {
        private readonly TalentDbContext _talentDbContext = talentDbContext;

        private readonly IAuthenticatedUserService _authenticatedUserService =
            authenticatedUserService;

        private readonly IDateTimeService _dateTimeService = dateTimeService;

        public async Task<Result<Response<TalentResponse>>> Handle(
            CreateTalentRequest request,
            CancellationToken cancellationToken
        )
        {
            return await SaveTalentAsync(request, cancellationToken)
                .BindAsync(talent => Task.FromResult(GenerateResponse(talent)));
        }

        private async Task<Result<Talent>> SaveTalentAsync(
            CreateTalentRequest request,
            CancellationToken cancellationToken
        )
        {
            var newTalent = new Talent(
                0,
                request.Name,
                request.BirthDate,
                request.BirthPlace,
                request.Nationality,
                request.Naturalness,
                request.LivesAbroad,
                request.RemoteJob,
                request.NumberOfChildren,
                request.FatherName,
                request.MotherName,
                request.CareerId,
                request.ValueOfServices,
                request.ConsortName,
                request.MaritalStateId,
                request.EducationLevelId,
                request.AddressLine1,
                request.AddressLine2,
                request.Email,
                request.CorporateEmail,
                request.PhoneNumber,
                request.EmergencyContact,
                request.LinkedIn,
                request.Fired,
                request.ResignationOpinion,
                request.Picture ?? Shared.Infrastructure.Resources.Images.PictureBase64Image,
                _dateTimeService.UtcNow,
                _authenticatedUserService.UserId,
                _dateTimeService.UtcNow,
                _authenticatedUserService.UserId
            )
            {
                Documents = request
                    .Documents.Select(document => new TalentDocument()
                    {
                        TalentId = document.TalentId,
                        DocumentTypeId = document.DocumentTypeId,
                        EnrollmentCode = document.EnrollmentCode,
                        EnrollmentDate = document.EnrollmentDate,
                        File = document.File,
                        Picture =
                            document.Picture
                            ?? Shared.Infrastructure.Resources.Images.DocumentBase64Image,
                        CreatedAt = _dateTimeService.UtcNow,
                        CreatedBy = _authenticatedUserService.UserId,
                        EditedAt = _dateTimeService.UtcNow,
                        EditedBy = _authenticatedUserService.UserId,
                    })
                    .ToList(),

                Skills = request
                    .Skills.Select(skill => new Skill()
                    {
                        TalentId = skill.TalentId,
                        Description = skill.Description,
                        Institute = skill.Institute,
                        SkillTypeId = skill.SkillTypeId,
                        SkillCategoryId = skill.SkillCategoryId,
                        SkillLevelId = skill.SkillLevelId,
                        SkillLevelMaxId = skill.SkillLevelMaxId,
                        SkillStateId = skill.SkillStateId,
                        StartDate = skill.StartDate,
                        EndDate = skill.EndDate,
                        Picture = skill.Picture,
                        CreatedAt = _dateTimeService.UtcNow,
                        CreatedBy = _authenticatedUserService.UserId,
                        EditedAt = _dateTimeService.UtcNow,
                        EditedBy = _authenticatedUserService.UserId,
                    })
                    .ToList(),
            };

            newTalent.AddEvent(new TalentCreatedEvent(newTalent.Id));

            await ExecuteTransactionAsync(
                async () =>
                {
                    await _talentDbContext.Talents.AddAsync(
                        newTalent,
                        cancellationToken: cancellationToken
                    );
                },
                newTalent.GetEvents(),
                cancellationToken: cancellationToken
            );

            return Result<Talent>.Success(newTalent);
        }

        private Result<Response<TalentResponse>> GenerateResponse(Talent talent)
        {
            var talentResponse = mapper.Map<TalentResponse>(talent);
            var response = new Response<TalentResponse>(talentResponse);

            return Result<Response<TalentResponse>>.Success(response);
        }
    }
}
