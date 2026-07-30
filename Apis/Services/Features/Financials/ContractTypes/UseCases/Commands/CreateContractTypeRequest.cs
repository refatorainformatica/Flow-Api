using MediatR;
using Services.Features.Financials.ContractTypes.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.ContractTypes.UseCases.Commands
{
    public class CreateContractTypeRequest
        : ContractTypeRequest,
            IRequest<Result<Response<ContractTypeResponse>>> { }
}
