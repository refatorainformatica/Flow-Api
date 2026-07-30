using Shared.Domain.Abstractions.Events;

namespace Services.Features.Projects.PriorityTypes.Events
{
    public class PriorityTypeCreatedEvent(int id) : DomainEvent
    {
        public int Id { get; } = id;
    }
}
