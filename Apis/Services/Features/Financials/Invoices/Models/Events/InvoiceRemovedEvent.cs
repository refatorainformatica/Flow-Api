using Shared.Domain.Abstractions.Events;

namespace Services.Features.Financials.Invoices.Models.Events
{
    public class InvoiceRemovedEvent(int id) : DomainEvent
    {
        public int Id { get; } = id;
    }
}
