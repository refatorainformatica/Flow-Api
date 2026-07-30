using MediatR;
using Services.Features.Financials.PaymentStates.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.PaymentStates.UseCases.Commands
{
    public class EditPaymentStateRequest
        : PaymentStateRequest,
            IRequest<Result<Response<PaymentStateResponse>>>
    {
        public int RequestId { get; set; }
    }
}
