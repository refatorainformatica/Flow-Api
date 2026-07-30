using MediatR;
using Services.Features.Financials.PaymentStates.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.PaymentStates.UseCases.Queries
{
    public class GetByIdPaymentStateRequest : IRequest<Result<Response<PaymentStateResponse>>>
    {
        public int Id { get; set; }
    }
}
