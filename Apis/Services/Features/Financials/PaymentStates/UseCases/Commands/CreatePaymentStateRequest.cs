using MediatR;
using Services.Features.Financials.PaymentStates.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.PaymentStates.UseCases.Commands
{
    public class CreatePaymentStateRequest
        : PaymentStateRequest,
            IRequest<Result<Response<PaymentStateResponse>>> { }
}
