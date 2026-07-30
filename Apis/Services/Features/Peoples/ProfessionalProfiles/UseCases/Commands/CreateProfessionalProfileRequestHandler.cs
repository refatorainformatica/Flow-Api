using AutoMapper;
using MediatR;
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
    public class CreateProfessionalProfileRequestHandler(
        IAuthenticatedUserService authenticatedUserService,
        IDateTimeService dateTimeService,
        IMapper mapper,
        IMediator mediator,
        ProfessionalProfileDbContext professionalprofileDbContext
    )
        : CommandHandler(professionalprofileDbContext, mediator),
            IRequestHandler<
                CreateProfessionalProfileRequest,
                Result<Response<ProfessionalProfileResponse>>
            >
    {
        private readonly ProfessionalProfileDbContext _professionalprofileDbContext =
            professionalprofileDbContext;

        private readonly IAuthenticatedUserService _authenticatedUserService =
            authenticatedUserService;

        private readonly IDateTimeService _dateTimeService = dateTimeService;

        public async Task<Result<Response<ProfessionalProfileResponse>>> Handle(
            CreateProfessionalProfileRequest request,
            CancellationToken cancellationToken
        )
        {
            return await SaveProfessionalProfileAsync(request, cancellationToken)
                .BindAsync(professionalprofile =>
                    Task.FromResult(GenerateResponse(professionalprofile))
                );
        }

        private async Task<Result<ProfessionalProfile>> SaveProfessionalProfileAsync(
            CreateProfessionalProfileRequest request,
            CancellationToken cancellationToken
        )
        {
            var newProfessionalProfile = new ProfessionalProfile(
                0,
                request.Description,
                request.HourlyValue,
                request.OvertimeValue,
                request.AdditionalHourlyValue,
                request.Picture ?? Shared.Infrastructure.Resources.Images.PictureBase64Image,
                _dateTimeService.UtcNow,
                _authenticatedUserService.UserId,
                _dateTimeService.UtcNow,
                _authenticatedUserService.UserId
            );

            newProfessionalProfile.AddEvent(
                new ProfessionalProfileCreatedEvent(newProfessionalProfile.Id)
            );

            await ExecuteTransactionAsync(
                async () =>
                {
                    await _professionalprofileDbContext.ProfessionalProfiles.AddAsync(
                        newProfessionalProfile,
                        cancellationToken: cancellationToken
                    );
                },
                newProfessionalProfile.GetEvents(),
                cancellationToken: cancellationToken
            );

            return Result<ProfessionalProfile>.Success(newProfessionalProfile);
        }

        private Result<Response<ProfessionalProfileResponse>> GenerateResponse(
            ProfessionalProfile professionalprofile
        )
        {
            var professionalprofileResponse = mapper.Map<ProfessionalProfileResponse>(
                professionalprofile
            );
            var response = new Response<ProfessionalProfileResponse>(professionalprofileResponse);

            return Result<Response<ProfessionalProfileResponse>>.Success(response);
        }
    }
}
