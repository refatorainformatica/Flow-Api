using Shared.Domain.Abstractions.Events;

namespace Services.Features.Projects.ProjectStates.Events
{
    public class ProjectStateCreatedEvent(int id) : DomainEvent
    {
        public int Id { get; } = id;
    }
}
