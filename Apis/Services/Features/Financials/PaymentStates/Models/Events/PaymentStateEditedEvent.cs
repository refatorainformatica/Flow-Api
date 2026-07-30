using Shared.Domain.Abstractions.Events;

namespace Services.Features.Financials.PaymentStates.Models.Events
{
    public class PaymentStateEditedEvent(int id) : DomainEvent
    {
        public int Id { get; } = id;
    }
}
