using MediatR;
using Services.Features.Peoples.SkillStates.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Peoples.SkillStates.UseCases.Commands
{
    public class EditSkillStateRequest
        : SkillStateRequest,
            IRequest<Result<Response<SkillStateResponse>>>
    {
        public int RequestId { get; set; }
    }
}
