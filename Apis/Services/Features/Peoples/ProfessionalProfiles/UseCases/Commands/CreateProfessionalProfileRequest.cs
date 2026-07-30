using MediatR;
using Services.Features.Peoples.ProfessionalProfiles.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Peoples.ProfessionalProfiles.UseCases.Commands
{
    public class CreateProfessionalProfileRequest
        : ProfessionalProfileRequest,
            IRequest<Result<Response<ProfessionalProfileResponse>>> { }
}
