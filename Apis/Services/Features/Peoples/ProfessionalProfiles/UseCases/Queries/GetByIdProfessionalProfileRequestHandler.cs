using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Features.Peoples.ProfessionalProfiles.Exceptions;
using Services.Features.Peoples.ProfessionalProfiles.Models;
using Services.Features.Peoples.ProfessionalProfiles.Repositories;
using Services.Features.Peoples.ProfessionalProfiles.UseCases.Queries;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.ProfessionalProfiles.UseCases.Queries
{
    public class GetByIdProfessionalProfileRequestHandler(
        IMapper mapper,
        IMediator mediator,
        ProfessionalProfileDbContext professionalprofileDbContext
    )
        : CommandHandler(professionalprofileDbContext, mediator),
            IRequestHandler<
                GetByIdProfessionalProfileRequest,
                Result<Response<ProfessionalProfileResponse>>
            >
    {
        private readonly ProfessionalProfileDbContext _professionalprofileDbContext =
            professionalprofileDbContext;

        public async Task<Result<Response<ProfessionalProfileResponse>>> Handle(
            GetByIdProfessionalProfileRequest request,
            CancellationToken cancellationToken
        )
        {
            return await GetByIdProfessionalProfileAsync(request, cancellationToken)
                .BindAsync(professionalprofiles =>
                    Task.FromResult(GenerateResponse(professionalprofiles))
                );
        }

        private async Task<Result<ProfessionalProfile>> GetByIdProfessionalProfileAsync(
            GetByIdProfessionalProfileRequest request,
            CancellationToken cancellationToken
        )
        {
            var professionalprofile = await _professionalprofileDbContext
                .ProfessionalProfiles.AsNoTracking()
                .FirstOrDefaultAsync(
                    x => x.Id == request.Id && !x.DeletedAt.HasValue,
                    cancellationToken
                );

            return professionalprofile is null
                ? Result<ProfessionalProfile>.Failure(
                    ProfessionalProfileErrors.NotFound(request.Id)
                )
                : Result<ProfessionalProfile>.Success(professionalprofile);
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
