using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.MovementTypes.Models
{
    public class MovementTypeResponse : BaseResponse
    {
        public string ExternalCode { get; set; }
        public string Description { get; set; }
    }
}
