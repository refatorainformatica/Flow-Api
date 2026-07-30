using Shared.Domain.Abstractions.Events;

namespace Services.Features.Projects.ProjectTaskStates.Events
{
    public class ProjectTaskStateCreatedEvent(int id) : DomainEvent
    {
        public int Id { get; } = id;
    }
}
