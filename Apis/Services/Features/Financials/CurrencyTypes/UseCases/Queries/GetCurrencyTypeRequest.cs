using MediatR;
using Services.Features.Financials.CurrencyTypes.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.CurrencyTypes.UseCases.Queries
{
    public class GetCurrencyTypeRequest
        : IRequest<Result<Response<IEnumerable<CurrencyTypeResponse>>>>
    {
        public BaseQuery Query { get; set; } = new();
    }
}
