using MediatR;
using Services.Features.Financials.ExpenseTypes.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.ExpenseTypes.UseCases.Queries
{
    public class GetExpenseTypeRequest
        : IRequest<Result<Response<IEnumerable<ExpenseTypeResponse>>>>
    {
        public BaseQuery Query { get; set; } = new();
    }
}
