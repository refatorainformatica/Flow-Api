using MediatR;
using Services.Features.Peoples.SkillTypes.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Peoples.SkillTypes.UseCases.Commands
{
    public class RemoveSkillTypeRequest : IRequest<Result<Response<SkillTypeResponse>>>
    {
        public int Id { get; set; }
    }
}
