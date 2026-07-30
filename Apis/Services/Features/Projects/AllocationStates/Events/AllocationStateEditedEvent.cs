using Shared.Domain.Abstractions.Events;

namespace Services.Features.Projects.AllocationStates.Events
{
    public class AllocationStateEditedEvent(int id) : DomainEvent
    {
        public int Id { get; } = id;
    }
}
