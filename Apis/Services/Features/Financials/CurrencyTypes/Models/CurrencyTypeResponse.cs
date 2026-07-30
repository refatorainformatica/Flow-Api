using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.CurrencyTypes.Models
{
    public class CurrencyTypeResponse : BaseResponse
    {
        public string Description { get; set; }
    }
}
