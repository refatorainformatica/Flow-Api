using Shared.Domain.Abstractions.Events;

namespace Services.Features.Financials.InvoiceTypes.Models.Events
{
    public class InvoiceTypeCreatedEvent(int id) : DomainEvent
    {
        public int Id { get; } = id;
    }
}
