using Shared.Domain.Abstractions.Events;

namespace Services.Features.Projects.Allocations.Events
{
    public class AllocationEditedEvent(int id) : DomainEvent
    {
        public int Id { get; } = id;
    }
}
