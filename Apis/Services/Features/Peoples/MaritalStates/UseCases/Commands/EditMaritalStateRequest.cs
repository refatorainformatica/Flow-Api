using MediatR;
using Services.Features.Peoples.MaritalStates.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Peoples.MaritalStates.UseCases.Commands
{
    public class EditMaritalStateRequest
        : MaritalStateRequest,
            IRequest<Result<Response<MaritalStateResponse>>>
    {
        public int RequestId { get; set; }
    }
}
