using MediatR;
using Services.Features.Peoples.SkillStates.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Peoples.SkillStates.UseCases.Queries
{
    public class GetBySearchSkillStateRequest
        : IRequest<Result<Response<IEnumerable<SkillStateResponse>>>>
    {
        public BaseQuerySearch Query { get; set; } = new();
    }
}
