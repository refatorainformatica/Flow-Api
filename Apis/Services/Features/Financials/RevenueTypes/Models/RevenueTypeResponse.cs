using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.RevenueTypes.Models
{
    public class RevenueTypeResponse : BaseResponse
    {
        public string Description { get; set; }
    }
}
