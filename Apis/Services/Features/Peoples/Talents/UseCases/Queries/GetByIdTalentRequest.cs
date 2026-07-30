using MediatR;
using Services.Features.Peoples.Talents.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Peoples.Talents.UseCases.Queries
{
    public class GetByIdTalentRequest : IRequest<Result<Response<TalentResponse>>>
    {
        public int Id { get; set; }
    }
}
