using MediatR;
using Services.Features.Peoples.Talents.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Peoples.Talents.UseCases.Commands
{
    public class CreateTalentRequest : TalentRequest, IRequest<Result<Response<TalentResponse>>> { }
}
