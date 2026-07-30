namespace Services.Features.Financials.InvoiceStates.Models
{
    public class InvoiceStateRequest
    {
        public int Id { get; set; }
        public string ExternalCode { get; set; }
        public string Description { get; set; }
        public string Picture { get; set; }
    }
}
