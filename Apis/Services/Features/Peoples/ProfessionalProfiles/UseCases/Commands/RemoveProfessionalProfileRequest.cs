using MediatR;
using Services.Features.Peoples.ProfessionalProfiles.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Peoples.ProfessionalProfiles.UseCases.Commands
{
    public class RemoveProfessionalProfileRequest
        : IRequest<Result<Response<ProfessionalProfileResponse>>>
    {
        public int Id { get; set; }
    }
}
