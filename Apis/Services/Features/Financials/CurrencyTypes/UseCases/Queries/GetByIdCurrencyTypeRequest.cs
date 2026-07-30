using MediatR;
using Services.Features.Financials.CurrencyTypes.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.CurrencyTypes.UseCases.Queries
{
    public class GetByIdCurrencyTypeRequest : IRequest<Result<Response<CurrencyTypeResponse>>>
    {
        public int Id { get; set; }
    }
}
