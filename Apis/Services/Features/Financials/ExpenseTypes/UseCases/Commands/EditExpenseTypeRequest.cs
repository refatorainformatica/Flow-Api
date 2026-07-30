using MediatR;
using Services.Features.Financials.ExpenseTypes.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.ExpenseTypes.UseCases.Commands
{
    public class EditExpenseTypeRequest
        : ExpenseTypeRequest,
            IRequest<Result<Response<ExpenseTypeResponse>>>
    {
        public int RequestId { get; set; }
    }
}
