using Shared.Domain.Abstractions.Events;

namespace Services.Features.Projects.ProjectTaskTypes.Events
{
    public class ProjectTaskTypeEditedEvent(int id) : DomainEvent
    {
        public int Id { get; } = id;
    }
}
