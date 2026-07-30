using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.CostCenters.Models
{
    public class CostCenterResponse : BaseResponse
    {
        public string Description { get; set; }
    }
}
