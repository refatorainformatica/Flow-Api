using MediatR;
using Services.Features.Peoples.EducationLevels.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Peoples.EducationLevels.UseCases.Queries
{
    public class GetBySearchEducationLevelRequest
        : IRequest<Result<Response<IEnumerable<EducationLevelResponse>>>>
    {
        public BaseQuerySearch Query { get; set; } = new();
    }
}
