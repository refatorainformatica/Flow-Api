using MediatR;
using Services.Features.Financials.MovementTypes.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.MovementTypes.UseCases.Commands
{
    public class CreateMovementTypeRequest
        : MovementTypeRequest,
            IRequest<Result<Response<MovementTypeResponse>>> { }
}
