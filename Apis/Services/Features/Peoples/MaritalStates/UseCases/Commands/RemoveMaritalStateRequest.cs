using MediatR;
using Services.Features.Peoples.MaritalStates.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Peoples.MaritalStates.UseCases.Commands
{
    public class RemoveMaritalStateRequest : IRequest<Result<Response<MaritalStateResponse>>>
    {
        public int Id { get; set; }
    }
}
