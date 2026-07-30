using MediatR;
using Services.Features.Financials.Contracts.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.Contracts.UseCases.Queries
{
    public class GetBySearchContractRequest
        : IRequest<Result<Response<IEnumerable<ContractResponse>>>>
    {
        public BaseQuerySearch Query { get; set; } = new();
    }
}
