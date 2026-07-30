using MediatR;
using Services.Features.Financials.MovementTypes.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.MovementTypes.UseCases.Commands
{
    public class EditMovementTypeRequest
        : MovementTypeRequest,
            IRequest<Result<Response<MovementTypeResponse>>>
    {
        public int RequestId { get; set; }
    }
}
