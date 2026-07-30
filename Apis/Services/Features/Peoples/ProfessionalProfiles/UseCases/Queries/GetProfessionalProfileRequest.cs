using MediatR;
using Services.Features.Peoples.ProfessionalProfiles.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Peoples.ProfessionalProfiles.UseCases.Queries
{
    public class GetProfessionalProfileRequest
        : IRequest<Result<Response<IEnumerable<ProfessionalProfileResponse>>>>
    {
        public BaseQuery Query { get; set; }
    }
}
