using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.ExpenseTypes.Models
{
    public class ExpenseTypeResponse : BaseResponse
    {
        public string Description { get; set; }
    }
}
