using MediatR;
using Services.Features.Financials.Expenses.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.Expenses.UseCases.Queries
{
    public class GetExpenseRequest : IRequest<Result<Response<IEnumerable<ExpenseResponse>>>>
    {
        public BaseQuery Query { get; set; } = new();
    }
}
