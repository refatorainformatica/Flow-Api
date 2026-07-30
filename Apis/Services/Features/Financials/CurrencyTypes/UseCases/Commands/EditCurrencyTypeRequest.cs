using MediatR;
using Services.Features.Financials.CurrencyTypes.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.CurrencyTypes.UseCases.Commands
{
    public class EditCurrencyTypeRequest
        : CurrencyTypeRequest,
            IRequest<Result<Response<CurrencyTypeResponse>>>
    {
        public int RequestId { get; set; }
    }
}
