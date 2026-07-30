using Shared.Domain.Abstractions.Events;

namespace Services.Features.Projects.Allocations.Events
{
    public class AllocationCreatedEvent(int id) : DomainEvent
    {
        public int Id { get; } = id;
    }
}
