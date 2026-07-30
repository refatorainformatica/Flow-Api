using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Features.Peoples.ProfessionalProfiles.Exceptions;
using Services.Features.Peoples.ProfessionalProfiles.Models;
using Services.Features.Peoples.ProfessionalProfiles.Models.Events;
using Services.Features.Peoples.ProfessionalProfiles.Repositories;
using Services.Features.Peoples.ProfessionalProfiles.UseCases.Commands;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.DateTime;
using Shared.Domain.Abstractions.Primitives;
using Shared.Domain.Abstractions.Security;

namespace Services.Features.Financials.ProfessionalProfiles.UseCases.Commands
{
    public class EditProfessionalProfileRequestHandler(
        IAuthenticatedUserService authenticatedUserService,
        IDateTimeService dateTimeService,
        IMediator mediator,
        ProfessionalProfileDbContext professionalprofileDbContext
    )
        : CommandHandler(professionalprofileDbContext, mediator),
            IRequestHandler<
                EditProfessionalProfileRequest,
                Result<Response<ProfessionalProfileResponse>>
            >
    {
        private readonly ProfessionalProfileDbContext _professionalprofileDbContext =
            professionalprofileDbContext;

        private readonly IAuthenticatedUserService _authenticatedUserService =
            authenticatedUserService;

        private readonly IDateTimeService _dateTimeService = dateTimeService;

        public async Task<Result<Response<ProfessionalProfileResponse>>> Handle(
            EditProfessionalProfileRequest request,
            CancellationToken cancellationToken
        )
        {
            return await ValidateRequest(request)
                .BindAsync(req => GetCurrentProfessionalProfileAsync(req.Id, cancellationToken))
                .BindAsync(currentProfessionalProfile =>
                    EditAndSaveProfessionalProfileAsync(
                        currentProfessionalProfile,
                        request,
                        cancellationToken
                    )
                )
                .MapAsync(currentProfessionalProfile =>
                {
                    return new Response<ProfessionalProfileResponse>(null);
                });
        }

        private static Result<EditProfessionalProfileRequest> ValidateRequest(
            EditProfessionalProfileRequest request
        )
        {
            return request.Id != request.RequestId
                ? Result<EditProfessionalProfileRequest>.Failure(
                    ProfessionalProfileErrors.PreConditionFailed(request.RequestId)
                )
                : Result<EditProfessionalProfileRequest>.Success(request);
        }

        private async Task<Result<ProfessionalProfile>> GetCurrentProfessionalProfileAsync(
            int id,
            CancellationToken cancellationToken
        )
        {
            var professionalprofile = await _professionalprofileDbContext
                .ProfessionalProfiles.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id && !x.DeletedAt.HasValue, cancellationToken);

            return professionalprofile is null
                ? Result<ProfessionalProfile>.Failure(ProfessionalProfileErrors.NotFound(id))
                : Result<ProfessionalProfile>.Success(professionalprofile);
        }

        private async Task<Result<ProfessionalProfile>> EditAndSaveProfessionalProfileAsync(
            ProfessionalProfile currentProfessionalProfile,
            EditProfessionalProfileRequest request,
            CancellationToken cancellationToken
        )
        {
            var editProfessionalProfile = new ProfessionalProfile(
                request.Id,
                request.Description,
                request.HourlyValue,
                request.OvertimeValue,
                request.AdditionalHourlyValue,
                request.Picture ?? Shared.Infrastructure.Resources.Images.PictureBase64Image,
                currentProfessionalProfile.CreatedAt.GetValueOrDefault(),
                currentProfessionalProfile.CreatedBy,
                _dateTimeService.UtcNow,
                _authenticatedUserService.UserId
            );

            editProfessionalProfile.AddEvent(
                new ProfessionalProfileEditedEvent(editProfessionalProfile.Id)
            );

            await ExecuteTransactionAsync(
                () =>
                    _professionalprofileDbContext.ProfessionalProfiles.Update(
                        editProfessionalProfile
                    ),
                editProfessionalProfile.GetEvents(),
                cancellationToken
            );

            return Result<ProfessionalProfile>.Success(editProfessionalProfile);
        }
    }
}
