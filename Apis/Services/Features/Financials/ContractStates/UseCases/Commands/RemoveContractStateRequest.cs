using MediatR;
using Services.Features.Financials.ContractStates.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.ContractStates.UseCases.Commands
{
    public class RemoveContractStateRequest : IRequest<Result<Response<ContractStateResponse>>>
    {
        public int Id { get; set; }
    }
}
