using MediatR;
using Services.Features.Peoples.JuridicalNatures.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Peoples.JuridicalNatures.UseCases.Queries
{
    public class GetJuridicalNatureRequest
        : IRequest<Result<Response<IEnumerable<JuridicalNatureResponse>>>>
    {
        public BaseQuery Query { get; set; }
    }
}
