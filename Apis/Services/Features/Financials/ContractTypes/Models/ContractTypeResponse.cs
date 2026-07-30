using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.ContractTypes.Models
{
    public class ContractTypeResponse : BaseResponse
    {
        public string ExternalCode { get; set; }
        public string Description { get; set; }
    }
}
