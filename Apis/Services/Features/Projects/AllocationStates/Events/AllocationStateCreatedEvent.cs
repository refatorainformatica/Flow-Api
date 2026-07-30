using Shared.Domain.Abstractions.Events;

namespace Services.Features.Projects.AllocationStates.Events
{
    public class AllocationStateCreatedEvent(int id) : DomainEvent
    {
        public int Id { get; } = id;
    }
}
