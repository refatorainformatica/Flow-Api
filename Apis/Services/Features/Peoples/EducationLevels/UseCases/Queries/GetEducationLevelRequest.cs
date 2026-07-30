using MediatR;
using Services.Features.Peoples.EducationLevels.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Peoples.EducationLevels.UseCases.Queries
{
    public class GetEducationLevelRequest
        : IRequest<Result<Response<IEnumerable<EducationLevelResponse>>>>
    {
        public BaseQuery Query { get; set; }
    }
}
