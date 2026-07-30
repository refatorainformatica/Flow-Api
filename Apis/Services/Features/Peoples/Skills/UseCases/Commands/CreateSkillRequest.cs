using MediatR;
using Services.Features.Peoples.Skills.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Peoples.Skills.UseCases.Commands
{
    public class CreateSkillRequest : SkillRequest, IRequest<Result<Response<SkillResponse>>> { }
}
