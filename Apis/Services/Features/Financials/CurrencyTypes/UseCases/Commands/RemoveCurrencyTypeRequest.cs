using MediatR;
using Services.Features.Financials.CurrencyTypes.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.CurrencyTypes.UseCases.Commands
{
    public class RemoveCurrencyTypeRequest : IRequest<Result<Response<CurrencyTypeResponse>>>
    {
        public int Id { get; set; }
    }
}
