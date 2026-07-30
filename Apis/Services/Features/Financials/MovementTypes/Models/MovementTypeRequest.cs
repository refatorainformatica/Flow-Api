namespace Services.Features.Financials.MovementTypes.Models
{
    public class MovementTypeRequest
    {
        public int Id { get; set; }
        public string ExternalCode { get; set; }
        public string Description { get; set; }
        public string Picture { get; set; }
    }
}
