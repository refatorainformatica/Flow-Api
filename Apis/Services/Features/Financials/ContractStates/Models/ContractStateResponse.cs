using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.ContractStates.Models
{
    public class ContractStateResponse : BaseResponse
    {
        public string ExternalCode { get; set; }
        public string Description { get; set; }
    }
}
