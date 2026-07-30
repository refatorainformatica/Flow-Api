using MediatR;
using Services.Features.Financials.Contracts.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.Contracts.UseCases.Commands
{
    public class EditContractRequest : ContractRequest, IRequest<Result<Response<ContractResponse>>>
    {
        public int RequestId { get; set; }
    }
}
