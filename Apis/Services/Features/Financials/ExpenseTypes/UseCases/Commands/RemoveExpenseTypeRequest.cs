using MediatR;
using Services.Features.Financials.ExpenseTypes.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.ExpenseTypes.UseCases.Commands
{
    public class RemoveExpenseTypeRequest : IRequest<Result<Response<ExpenseTypeResponse>>>
    {
        public int Id { get; set; }
    }
}
