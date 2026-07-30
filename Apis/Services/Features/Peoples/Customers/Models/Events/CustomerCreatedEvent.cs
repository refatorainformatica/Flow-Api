using Shared.Domain.Abstractions.Events;

namespace Services.Features.Peoples.Customers.Models.Events
{
    public class CustomerCreatedEvent(int id) : DomainEvent
    {
        public int Id { get; } = id;
    }
}
