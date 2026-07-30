using MediatR;
using Services.Features.Financials.Expenses.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.Expenses.UseCases.Commands
{
    public class RemoveExpenseRequest : IRequest<Result<Response<ExpenseResponse>>>
    {
        public int Id { get; set; }
    }
}
