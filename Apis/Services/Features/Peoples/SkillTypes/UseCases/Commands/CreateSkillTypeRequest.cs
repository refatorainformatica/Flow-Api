using MediatR;
using Services.Features.Peoples.SkillTypes.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Peoples.SkillTypes.UseCases.Commands
{
    public class CreateSkillTypeRequest
        : SkillTypeRequest,
            IRequest<Result<Response<SkillTypeResponse>>> { }
}
