using Shared.Domain.Abstractions.Events;

namespace Services.Features.Financials.InvoiceStates.Models.Events
{
    public class InvoiceStateRemovedEvent(int id) : DomainEvent
    {
        public int Id { get; } = id;
    }
}
