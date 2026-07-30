namespace Services.Features.Financials.PaymentStates.Models
{
    public class PaymentStateRequest
    {
        public int Id { get; set; }
        public string ExternalCode { get; set; }
        public string Description { get; set; }
        public string Picture { get; set; }
    }
}
