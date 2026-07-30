using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Features.Peoples.Skills.Models;
using Services.Features.Peoples.Talents.Exceptions;
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
    public class EditTalentRequestHandler(
        IAuthenticatedUserService authenticatedUserService,
        IDateTimeService dateTimeService,
        IMediator mediator,
        TalentDbContext talentDbContext
    )
        : CommandHandler(talentDbContext, mediator),
            IRequestHandler<EditTalentRequest, Result<Response<TalentResponse>>>
    {
        private readonly TalentDbContext _talentDbContext = talentDbContext;

        private readonly IAuthenticatedUserService _authenticatedUserService =
            authenticatedUserService;

        private readonly IDateTimeService _dateTimeService = dateTimeService;

        public async Task<Result<Response<TalentResponse>>> Handle(
            EditTalentRequest request,
            CancellationToken cancellationToken
        )
        {
            return await ValidateRequest(request)
                .BindAsync(req => GetCurrentTalentAsync(req.Id, cancellationToken))
                .BindAsync(currentTalent =>
                    EditAndSaveTalentAsync(currentTalent, request, cancellationToken)
                )
                .MapAsync(currentTalent =>
                {
                    return new Response<TalentResponse>(null);
                });
        }

        private static Result<EditTalentRequest> ValidateRequest(EditTalentRequest request)
        {
            return request.Id != request.RequestId
                ? Result<EditTalentRequest>.Failure(
                    TalentErrors.PreConditionFailed(request.RequestId)
                )
                : Result<EditTalentRequest>.Success(request);
        }

        private async Task<Result<Talent>> GetCurrentTalentAsync(
            int id,
            CancellationToken cancellationToken
        )
        {
            var talent = await _talentDbContext
                .Talents.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id && !x.DeletedAt.HasValue, cancellationToken);

            return talent is null
                ? Result<Talent>.Failure(TalentErrors.NotFound(id))
                : Result<Talent>.Success(talent);
        }

        private async Task<Result<Talent>> EditAndSaveTalentAsync(
            Talent currentTalent,
            EditTalentRequest request,
            CancellationToken cancellationToken
        )
        {
            var editTalent = new Talent(
                request.Id,
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
                currentTalent.CreatedAt.GetValueOrDefault(),
                currentTalent.CreatedBy,
                _dateTimeService.UtcNow,
                _authenticatedUserService.UserId
            )
            {
                Documents = request
                    .Documents.Select(document => new TalentDocument()
                    {
                        Id =
                            currentTalent
                                .Documents.FirstOrDefault(x =>
                                    x.DocumentTypeId == document.DocumentTypeId
                                )
                                ?.Id ?? 0,
                        TalentId = document.TalentId,
                        DocumentTypeId = document.DocumentTypeId,
                        EnrollmentCode = document.EnrollmentCode,
                        EnrollmentDate = document.EnrollmentDate,
                        File = document.File,
                        Picture =
                            document.Picture
                            ?? Shared.Infrastructure.Resources.Images.DocumentBase64Image,
                        CreatedAt =
                            currentTalent
                                .Documents.FirstOrDefault(x =>
                                    x.DocumentTypeId == document.DocumentTypeId
                                )
                                ?.CreatedAt ?? _dateTimeService.UtcNow,
                        CreatedBy =
                            currentTalent
                                .Documents.FirstOrDefault(x =>
                                    x.DocumentTypeId == document.DocumentTypeId
                                )
                                ?.CreatedBy ?? _authenticatedUserService.UserId,
                        EditedAt = _dateTimeService.UtcNow,
                        EditedBy = _authenticatedUserService.UserId,
                        DeletedAt = document.DeletedAt,
                    })
                    .ToList(),

                Skills = request
                    .Skills.Select(skill => new Skill()
                    {
                        Id =
                            currentTalent
                                .Skills.FirstOrDefault(x =>
                                    x.Description == skill.Description
                                    && x.Institute == skill.Institute
                                    && x.SkillTypeId == skill.SkillTypeId
                                )
                                ?.Id ?? 0,
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
                        CreatedAt =
                            currentTalent
                                .Skills.FirstOrDefault(x =>
                                    x.Description == skill.Description
                                    && x.Institute == skill.Institute
                                    && x.SkillTypeId == skill.SkillTypeId
                                )
                                ?.CreatedAt ?? _dateTimeService.UtcNow,
                        CreatedBy =
                            currentTalent
                                .Skills.FirstOrDefault(x =>
                                    x.Description == skill.Description
                                    && x.Institute == skill.Institute
                                    && x.SkillTypeId == skill.SkillTypeId
                                )
                                ?.CreatedBy ?? _authenticatedUserService.UserId,
                        EditedAt = _dateTimeService.UtcNow,
                        EditedBy = _authenticatedUserService.UserId,
                    })
                    .ToList(),
            };

            editTalent.AddEvent(new TalentEditedEvent(editTalent.Id));

            await ExecuteTransactionAsync(
                () => _talentDbContext.Talents.Update(editTalent),
                editTalent.GetEvents(),
                cancellationToken
            );

            return Result<Talent>.Success(editTalent);
        }
    }
}
