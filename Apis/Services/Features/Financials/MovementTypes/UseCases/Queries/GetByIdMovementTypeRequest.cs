using MediatR;
using Services.Features.Financials.MovementTypes.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.MovementTypes.UseCases.Queries
{
    public class GetByIdMovementTypeRequest : IRequest<Result<Response<MovementTypeResponse>>>
    {
        public int Id { get; set; }
    }
}
