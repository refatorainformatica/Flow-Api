namespace Services.Features.Financials.ContractTypes.Models
{
    public class ContractTypeRequest
    {
        public int Id { get; set; }
        public string ExternalCode { get; set; }
        public string Description { get; set; }
        public string Picture { get; set; }
    }
}
