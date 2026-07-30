using MediatR;
using Services.Features.Financials.PaymentStates.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.PaymentStates.UseCases.Queries
{
    public class GetPaymentStateRequest
        : IRequest<Result<Response<IEnumerable<PaymentStateResponse>>>>
    {
        public BaseQuery Query { get; set; } = new();
    }
}
