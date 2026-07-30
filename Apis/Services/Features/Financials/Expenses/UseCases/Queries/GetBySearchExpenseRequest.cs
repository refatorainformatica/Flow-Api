using MediatR;
using Services.Features.Financials.Expenses.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.Expenses.UseCases.Queries
{
    public class GetBySearchExpenseRequest
        : IRequest<Result<Response<IEnumerable<ExpenseResponse>>>>
    {
        public BaseQuerySearch Query { get; set; } = new();
    }
}
