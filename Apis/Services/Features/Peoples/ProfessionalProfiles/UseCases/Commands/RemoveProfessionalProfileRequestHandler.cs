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
    public class RemoveProfessionalProfileRequestHandler(
        IAuthenticatedUserService authenticatedUserService,
        IDateTimeService dateTimeService,
        IMediator mediator,
        ProfessionalProfileDbContext professionalprofileDbContext
    )
        : CommandHandler(professionalprofileDbContext, mediator),
            IRequestHandler<
                RemoveProfessionalProfileRequest,
                Result<Response<ProfessionalProfileResponse>>
            >
    {
        private readonly ProfessionalProfileDbContext _professionalprofileDbContext =
            professionalprofileDbContext;

        private readonly IAuthenticatedUserService _authenticatedUserService =
            authenticatedUserService;

        private readonly IDateTimeService _dateTimeService = dateTimeService;

        public async Task<Result<Response<ProfessionalProfileResponse>>> Handle(
            RemoveProfessionalProfileRequest request,
            CancellationToken cancellationToken
        )
        {
            return await ValidateRequest(request)
                .BindAsync(req => GetCurrentProfessionalProfileAsync(req.Id, cancellationToken))
                .BindAsync(currentProfessionalProfile =>
                    RemoveProfessionalProfileAsync(currentProfessionalProfile, cancellationToken)
                )
                .MapAsync(currentProfessionalProfile =>
                {
                    return new Response<ProfessionalProfileResponse>(null);
                });
        }

        private static Result<RemoveProfessionalProfileRequest> ValidateRequest(
            RemoveProfessionalProfileRequest request
        )
        {
            return request.Id == default
                ? Result<RemoveProfessionalProfileRequest>.Failure(
                    ProfessionalProfileErrors.NotFound(request.Id)
                )
                : Result<RemoveProfessionalProfileRequest>.Success(request);
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

        private async Task<Result<ProfessionalProfile>> RemoveProfessionalProfileAsync(
            ProfessionalProfile removeProfessionalProfile,
            CancellationToken cancellationToken
        )
        {
            removeProfessionalProfile.DeletedAt = _dateTimeService.UtcNow;
            removeProfessionalProfile.EditedAt = _dateTimeService.UtcNow;
            removeProfessionalProfile.EditedBy = _authenticatedUserService.UserId;

            removeProfessionalProfile.AddEvent(
                new ProfessionalProfileRemovedEvent(removeProfessionalProfile.Id)
            );

            await ExecuteTransactionAsync(
                () => _professionalprofileDbContext.Update(removeProfessionalProfile),
                removeProfessionalProfile.GetEvents(),
                cancellationToken
            );

            return Result<ProfessionalProfile>.Success(removeProfessionalProfile);
        }
    }
}
