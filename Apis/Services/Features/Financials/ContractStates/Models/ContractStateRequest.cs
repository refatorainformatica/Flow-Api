namespace Services.Features.Financials.ContractStates.Models
{
    public class ContractStateRequest
    {
        public int Id { get; set; }
        public string ExternalCode { get; set; }
        public string Description { get; set; }
        public string Picture { get; set; }
    }
}
