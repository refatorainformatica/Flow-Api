using MediatR;
using Services.Features.Financials.PaymentStates.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.PaymentStates.UseCases.Queries
{
    public class GetBySearchPaymentStateRequest
        : IRequest<Result<Response<IEnumerable<PaymentStateResponse>>>>
    {
        public BaseQuerySearch Query { get; set; } = new();
    }
}
