using Shared.Domain.Abstractions.Events;

namespace Services.Features.Projects.Sprints.Events
{
    public class SprintRemovedEvent(int id) : DomainEvent
    {
        public int Id { get; } = id;
    }
}
