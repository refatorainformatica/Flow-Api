using MediatR;
using Services.Features.Peoples.SkillTypes.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Peoples.SkillTypes.UseCases.Queries
{
    public class GetSkillTypeRequest : IRequest<Result<Response<IEnumerable<SkillTypeResponse>>>>
    {
        public BaseQuery Query { get; set; }
    }
}
