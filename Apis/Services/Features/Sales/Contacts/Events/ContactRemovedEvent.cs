using Shared.Domain.Abstractions.Events;

namespace Services.Features.Sales.Contacts.Events
{
    public class ContactRemovedEvent(int id) : DomainEvent
    {
        public int Id { get; } = id;
    }
}
