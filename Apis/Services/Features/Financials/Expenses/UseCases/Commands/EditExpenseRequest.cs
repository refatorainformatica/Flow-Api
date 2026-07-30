using MediatR;
using Services.Features.Financials.Expenses.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.Expenses.UseCases.Commands
{
    public class EditExpenseRequest : ExpenseRequest, IRequest<Result<Response<ExpenseResponse>>>
    {
        public int RequestId { get; set; }
    }
}
