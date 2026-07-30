using MediatR;
using Services.Features.Financials.ContractTypes.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.ContractTypes.UseCases.Queries
{
    public class GetContractTypeRequest
        : IRequest<Result<Response<IEnumerable<ContractTypeResponse>>>>
    {
        public BaseQuery Query { get; set; } = new();
    }
}
