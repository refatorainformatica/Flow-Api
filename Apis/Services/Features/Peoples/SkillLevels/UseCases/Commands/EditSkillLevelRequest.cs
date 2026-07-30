using MediatR;
using Services.Features.Peoples.SkillLevels.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Peoples.SkillLevels.UseCases.Commands
{
    public class EditSkillLevelRequest
        : SkillLevelRequest,
            IRequest<Result<Response<SkillLevelResponse>>>
    {
        public int RequestId { get; set; }
    }
}
