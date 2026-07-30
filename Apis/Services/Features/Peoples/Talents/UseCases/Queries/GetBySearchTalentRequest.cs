using MediatR;
using Services.Features.Peoples.Talents.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Peoples.Talents.UseCases.Queries
{
    public class GetBySearchTalentRequest : IRequest<Result<Response<IEnumerable<TalentResponse>>>>
    {
        public BaseQuerySearch Query { get; set; } = new();
    }
}
