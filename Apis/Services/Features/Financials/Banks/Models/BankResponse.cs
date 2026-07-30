using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.Banks.Models
{
    public class BankResponse : BaseResponse
    {
        public string ExternalCode { get; set; }
        public string Description { get; set; }
    }
}
