using MediatR;
using Services.Features.Financials.ContractTypes.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.ContractTypes.UseCases.Commands
{
    public class EditContractTypeRequest
        : ContractTypeRequest,
            IRequest<Result<Response<ContractTypeResponse>>>
    {
        public int RequestId { get; set; }
    }
}
