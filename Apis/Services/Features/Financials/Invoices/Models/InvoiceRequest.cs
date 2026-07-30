namespace Services.Features.Financials.Invoices.Models
{
    public class InvoiceRequest
    {
        public int Id { get; set; }
        public int SupplierId { get; set; }
        public int InvoiceTypeId { get; set; }
        public int InvoiceStateId { get; set; }
        public DateTime DateOfIssue { get; set; }
        public string File { get; set; }
        public string Picture { get; set; }
    }
}
