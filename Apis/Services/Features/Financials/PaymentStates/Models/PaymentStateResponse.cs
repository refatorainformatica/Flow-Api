using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.PaymentStates.Models
{
    public class PaymentStateResponse : BaseResponse
    {
        public string ExternalCode { get; set; }
        public string Description { get; set; }
    }
}
