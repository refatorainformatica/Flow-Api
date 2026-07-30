using MediatR;
using Services.Features.Financials.ContractStates.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.ContractStates.UseCases.Queries
{
    public class GetBySearchContractStateRequest
        : IRequest<Result<Response<IEnumerable<ContractStateResponse>>>>
    {
        public BaseQuerySearch Query { get; set; } = new();
    }
}
