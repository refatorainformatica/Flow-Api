using MediatR;
using Services.Features.Financials.ExpenseTypes.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.ExpenseTypes.UseCases.Queries
{
    public class GetByIdExpenseTypeRequest : IRequest<Result<Response<ExpenseTypeResponse>>>
    {
        public int Id { get; set; }
    }
}
