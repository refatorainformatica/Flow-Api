using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.Invoices.Models
{
    public class InvoiceResponse : BaseResponse
    {
        public int SupplierId { get; set; }
        public int InvoiceTypeId { get; set; }
        public int InvoiceStateId { get; set; }
        public DateTime DateOfIssue { get; set; }
        public string File { get; set; }
    }
}
