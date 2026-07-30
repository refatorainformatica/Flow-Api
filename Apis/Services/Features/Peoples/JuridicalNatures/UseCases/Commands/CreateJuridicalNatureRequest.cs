using MediatR;
using Services.Features.Peoples.JuridicalNatures.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Peoples.JuridicalNatures.UseCases.Commands
{
    public class CreateJuridicalNatureRequest
        : JuridicalNatureRequest,
            IRequest<Result<Response<JuridicalNatureResponse>>> { }
}
