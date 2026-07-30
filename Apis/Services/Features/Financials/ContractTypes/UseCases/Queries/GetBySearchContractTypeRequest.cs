using MediatR;
using Services.Features.Financials.ContractTypes.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.ContractTypes.UseCases.Queries
{
    public class GetBySearchContractTypeRequest
        : IRequest<Result<Response<IEnumerable<ContractTypeResponse>>>>
    {
        public BaseQuerySearch Query { get; set; } = new();
    }
}
