using MediatR;
using Services.Features.Peoples.SkillLevels.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Peoples.SkillLevels.UseCases.Queries
{
    public class GetBySearchSkillLevelRequest
        : IRequest<Result<Response<IEnumerable<SkillLevelResponse>>>>
    {
        public BaseQuerySearch Query { get; set; } = new();
    }
}
