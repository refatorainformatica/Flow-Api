using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.InvoiceTypes.Models
{
    public class InvoiceTypeRequest : BaseResponse
    {
        public string Description { get; set; }
    }
}
