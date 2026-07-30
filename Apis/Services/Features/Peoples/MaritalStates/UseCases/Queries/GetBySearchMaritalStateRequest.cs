using MediatR;
using Services.Features.Peoples.MaritalStates.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Peoples.MaritalStates.UseCases.Queries
{
    public class GetBySearchMaritalStateRequest
        : IRequest<Result<Response<IEnumerable<MaritalStateResponse>>>>
    {
        public BaseQuerySearch Query { get; set; } = new();
    }
}
