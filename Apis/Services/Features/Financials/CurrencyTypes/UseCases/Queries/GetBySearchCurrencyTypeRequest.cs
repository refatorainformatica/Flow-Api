using MediatR;
using Services.Features.Financials.CurrencyTypes.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.CurrencyTypes.UseCases.Queries
{
    public class GetBySearchCurrencyTypeRequest
        : IRequest<Result<Response<IEnumerable<CurrencyTypeResponse>>>>
    {
        public BaseQuerySearch Query { get; set; } = new();
    }
}
