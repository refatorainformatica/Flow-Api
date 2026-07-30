using Shared.Domain.Abstractions.Events;

namespace Services.Features.Projects.Sprints.Events
{
    public class SprintEditedEvent(int id) : DomainEvent
    {
        public int Id { get; } = id;
    }
}
