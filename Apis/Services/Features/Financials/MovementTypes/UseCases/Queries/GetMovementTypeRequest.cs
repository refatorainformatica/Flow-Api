using MediatR;
using Services.Features.Financials.MovementTypes.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.MovementTypes.UseCases.Queries
{
    public class GetMovementTypeRequest
        : IRequest<Result<Response<IEnumerable<MovementTypeResponse>>>>
    {
        public BaseQuery Query { get; set; } = new();
    }
}
