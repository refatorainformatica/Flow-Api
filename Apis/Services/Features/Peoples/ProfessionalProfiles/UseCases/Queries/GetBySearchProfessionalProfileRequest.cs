using MediatR;
using Services.Features.Peoples.ProfessionalProfiles.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Peoples.ProfessionalProfiles.UseCases.Queries
{
    public class GetBySearchProfessionalProfileRequest
        : IRequest<Result<Response<IEnumerable<ProfessionalProfileResponse>>>>
    {
        public BaseQuerySearch Query { get; set; } = new();
    }
}
