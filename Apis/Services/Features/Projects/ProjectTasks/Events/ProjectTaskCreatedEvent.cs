using Shared.Domain.Abstractions.Events;

namespace Services.Features.Projects.ProjectTasks.Events
{
    public class ProjectTaskCreatedEvent(int id) : DomainEvent
    {
        public int Id { get; } = id;
    }
}
