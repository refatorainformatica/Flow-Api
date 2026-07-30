using MediatR;
using Services.Features.Peoples.SkillStates.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Peoples.SkillStates.UseCases.Commands
{
    public class RemoveSkillStateRequest : IRequest<Result<Response<SkillStateResponse>>>
    {
        public int Id { get; set; }
    }
}
