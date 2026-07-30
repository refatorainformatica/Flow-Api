using MediatR;
using Services.Features.Peoples.JuridicalNatures.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Peoples.JuridicalNatures.UseCases.Commands
{
    public class EditJuridicalNatureRequest
        : JuridicalNatureRequest,
            IRequest<Result<Response<JuridicalNatureResponse>>>
    {
        public int RequestId { get; set; }
    }
}
