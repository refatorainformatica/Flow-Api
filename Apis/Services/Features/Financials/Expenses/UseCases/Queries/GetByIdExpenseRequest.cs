using MediatR;
using Services.Features.Financials.Expenses.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.Expenses.UseCases.Queries
{
    public class GetByIdExpenseRequest : IRequest<Result<Response<ExpenseResponse>>>
    {
        public int Id { get; set; }
    }
}
