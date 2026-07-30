using MediatR;
using Services.Features.Financials.Contracts.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.Contracts.UseCases.Queries
{
    public class GetByIdContractRequest : IRequest<Result<Response<ContractResponse>>>
    {
        public int Id { get; set; }
    }
}
