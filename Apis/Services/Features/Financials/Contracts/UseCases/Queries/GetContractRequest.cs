using MediatR;
using Services.Features.Financials.Contracts.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.Contracts.UseCases.Queries
{
    public class GetContractRequest : IRequest<Result<Response<IEnumerable<ContractResponse>>>>
    {
        public BaseQuery Query { get; set; } = new();
    }
}
