using MediatR;
using Services.Features.Peoples.MaritalStates.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Peoples.MaritalStates.UseCases.Queries
{
    public class GetMaritalStateRequest
        : IRequest<Result<Response<IEnumerable<MaritalStateResponse>>>>
    {
        public BaseQuery Query { get; set; }
    }
}
