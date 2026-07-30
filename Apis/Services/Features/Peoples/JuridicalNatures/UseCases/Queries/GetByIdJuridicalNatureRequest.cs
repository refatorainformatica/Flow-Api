using MediatR;
using Services.Features.Peoples.JuridicalNatures.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Peoples.JuridicalNatures.UseCases.Queries
{
    public class GetByIdJuridicalNatureRequest : IRequest<Result<Response<JuridicalNatureResponse>>>
    {
        public int Id { get; set; }
    }
}
