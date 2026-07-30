using MediatR;
using Services.Features.Financials.ContractTypes.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.ContractTypes.UseCases.Queries
{
    public class GetByIdContractTypeRequest : IRequest<Result<Response<ContractTypeResponse>>>
    {
        public int Id { get; set; }
    }
}
