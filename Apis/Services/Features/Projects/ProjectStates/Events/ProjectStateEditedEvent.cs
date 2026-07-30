using Shared.Domain.Abstractions.Events;

namespace Services.Features.Projects.ProjectStates.Events
{
    public class ProjectStateEditedEvent(int id) : DomainEvent
    {
        public int Id { get; } = id;
    }
}
