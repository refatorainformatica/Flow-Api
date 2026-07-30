using MediatR;
using Services.Features.Peoples.Skills.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Peoples.Skills.UseCases.Commands
{
    public class RemoveSkillRequest : IRequest<Result<Response<SkillResponse>>>
    {
        public int Id { get; set; }
    }
}
