using MediatR;
using Services.Features.Peoples.JuridicalNatures.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Peoples.JuridicalNatures.UseCases.Queries
{
    public class GetBySearchJuridicalNatureRequest
        : IRequest<Result<Response<IEnumerable<JuridicalNatureResponse>>>>
    {
        public BaseQuerySearch Query { get; set; } = new();
    }
}
