using Shared.Domain.Abstractions.Events;

namespace Services.Features.Peoples.Customers.Models.Events
{
    public class CustomerEditedEvent(int id) : DomainEvent
    {
        public int Id { get; } = id;
    }
}
