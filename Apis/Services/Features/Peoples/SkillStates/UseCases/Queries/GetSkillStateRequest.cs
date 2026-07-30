using MediatR;
using Services.Features.Peoples.SkillStates.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Peoples.SkillStates.UseCases.Queries
{
    public class GetSkillStateRequest : IRequest<Result<Response<IEnumerable<SkillStateResponse>>>>
    {
        public BaseQuery Query { get; set; }
    }
}
