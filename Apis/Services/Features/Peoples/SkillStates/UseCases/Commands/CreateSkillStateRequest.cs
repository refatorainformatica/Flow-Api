using MediatR;
using Services.Features.Peoples.SkillStates.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Peoples.SkillStates.UseCases.Commands
{
    public class CreateSkillStateRequest
        : SkillStateRequest,
            IRequest<Result<Response<SkillStateResponse>>> { }
}
