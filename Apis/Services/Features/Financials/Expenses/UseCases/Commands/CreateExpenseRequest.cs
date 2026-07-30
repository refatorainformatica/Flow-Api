using MediatR;
using Services.Features.Financials.Expenses.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.Expenses.UseCases.Commands
{
    public class CreateExpenseRequest
        : ExpenseRequest,
            IRequest<Result<Response<ExpenseResponse>>> { }
}
