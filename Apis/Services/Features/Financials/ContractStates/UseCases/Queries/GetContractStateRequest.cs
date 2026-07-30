using MediatR;
using Services.Features.Financials.ContractStates.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.ContractStates.UseCases.Queries
{
    public class GetContractStateRequest
        : IRequest<Result<Response<IEnumerable<ContractStateResponse>>>>
    {
        public BaseQuery Query { get; set; } = new();
    }
}
