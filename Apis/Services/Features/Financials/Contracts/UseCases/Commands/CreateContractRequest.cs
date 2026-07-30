using MediatR;
using Services.Features.Financials.Contracts.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.Contracts.UseCases.Commands
{
    public class CreateContractRequest
        : ContractRequest,
            IRequest<Result<Response<ContractResponse>>> { }
}
