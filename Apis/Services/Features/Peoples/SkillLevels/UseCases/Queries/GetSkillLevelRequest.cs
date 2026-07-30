using MediatR;
using Services.Features.Peoples.SkillLevels.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Peoples.SkillLevels.UseCases.Queries
{
    public class GetSkillLevelRequest : IRequest<Result<Response<IEnumerable<SkillLevelResponse>>>>
    {
        public BaseQuery Query { get; set; }
    }
}
