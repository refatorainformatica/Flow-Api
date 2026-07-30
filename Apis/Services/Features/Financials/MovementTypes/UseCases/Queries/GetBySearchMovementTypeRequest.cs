using MediatR;
using Services.Features.Financials.MovementTypes.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.MovementTypes.UseCases.Queries
{
    public class GetBySearchMovementTypeRequest
        : IRequest<Result<Response<IEnumerable<MovementTypeResponse>>>>
    {
        public BaseQuerySearch Query { get; set; } = new();
    }
}
