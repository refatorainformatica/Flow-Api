using Shared.Domain.Abstractions.Events;

namespace Services.Features.Financials.InvoiceStates.Models.Events
{
    public class InvoiceStateEditedEvent(int id) : DomainEvent
    {
        public int Id { get; } = id;
    }
}
