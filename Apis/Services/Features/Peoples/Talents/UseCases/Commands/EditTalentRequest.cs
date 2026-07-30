using MediatR;
using Services.Features.Peoples.Talents.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Peoples.Talents.UseCases.Commands
{
    public class EditTalentRequest : TalentRequest, IRequest<Result<Response<TalentResponse>>>
    {
        public int RequestId { get; set; }
    }
}
