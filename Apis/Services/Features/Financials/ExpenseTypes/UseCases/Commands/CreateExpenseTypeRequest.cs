using MediatR;
using Services.Features.Financials.ExpenseTypes.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.ExpenseTypes.UseCases.Commands
{
    public class CreateExpenseTypeRequest
        : ExpenseTypeRequest,
            IRequest<Result<Response<ExpenseTypeResponse>>> { }
}
