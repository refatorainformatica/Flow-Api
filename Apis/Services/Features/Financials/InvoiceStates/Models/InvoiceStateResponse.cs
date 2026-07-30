using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.InvoiceStates.Models
{
    public class InvoiceStateResponse : BaseResponse
    {
        public string ExternalCode { get; set; }
        public string Description { get; set; }
    }
}
