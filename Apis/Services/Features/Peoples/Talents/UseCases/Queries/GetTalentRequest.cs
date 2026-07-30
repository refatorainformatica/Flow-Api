using MediatR;
using Services.Features.Peoples.Talents.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Peoples.Talents.UseCases.Queries
{
    public class GetTalentRequest : IRequest<Result<Response<IEnumerable<TalentResponse>>>>
    {
        public BaseQuery Query { get; set; }
    }
}
