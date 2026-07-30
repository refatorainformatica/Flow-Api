using Shared.Domain.Abstractions.Events;

namespace Services.Features.Sales.Contacts.Events
{
    public class ContactEditedEvent(int id) : DomainEvent
    {
        public int Id { get; } = id;
    }
}
