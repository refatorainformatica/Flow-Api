using MediatR;
using Services.Features.Financials.ContractStates.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.ContractStates.UseCases.Commands
{
    public class CreateContractStateRequest
        : ContractStateRequest,
            IRequest<Result<Response<ContractStateResponse>>> { }
}
