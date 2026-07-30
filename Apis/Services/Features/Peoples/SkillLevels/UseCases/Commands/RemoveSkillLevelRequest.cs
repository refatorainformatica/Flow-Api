using MediatR;
using Services.Features.Peoples.SkillLevels.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Peoples.SkillLevels.UseCases.Commands
{
    public class RemoveSkillLevelRequest : IRequest<Result<Response<SkillLevelResponse>>>
    {
        public int Id { get; set; }
    }
}
