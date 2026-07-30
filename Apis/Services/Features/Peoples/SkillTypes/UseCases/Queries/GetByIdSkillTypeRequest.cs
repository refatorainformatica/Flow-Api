using MediatR;
using Services.Features.Peoples.SkillTypes.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Peoples.SkillTypes.UseCases.Queries
{
    public class GetByIdSkillTypeRequest : IRequest<Result<Response<SkillTypeResponse>>>
    {
        public int Id { get; set; }
    }
}
