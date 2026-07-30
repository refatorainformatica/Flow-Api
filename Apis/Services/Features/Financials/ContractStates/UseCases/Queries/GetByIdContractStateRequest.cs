using MediatR;
using Services.Features.Financials.ContractStates.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.ContractStates.UseCases.Queries
{
    public class GetByIdContractStateRequest : IRequest<Result<Response<ContractStateResponse>>>
    {
        public int Id { get; set; }
    }
}
