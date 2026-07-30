using MediatR;
using Services.Features.Peoples.Talents.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Peoples.Talents.UseCases.Commands
{
    public class RemoveTalentRequest : IRequest<Result<Response<TalentResponse>>>
    {
        public int Id { get; set; }
    }
}
