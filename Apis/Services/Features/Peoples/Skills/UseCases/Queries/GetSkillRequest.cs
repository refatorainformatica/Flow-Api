using MediatR;
using Services.Features.Peoples.Skills.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Peoples.Skills.UseCases.Queries
{
    public class GetSkillRequest : IRequest<Result<Response<IEnumerable<SkillResponse>>>>
    {
        public BaseQuery Query { get; set; }
    }
}
