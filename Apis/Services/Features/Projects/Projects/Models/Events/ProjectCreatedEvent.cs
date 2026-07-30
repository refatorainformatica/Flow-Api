using Shared.Domain.Abstractions.Events;

namespace Services.Features.Projects.Projects.Models.Events
{
    public class ProjectCreatedEvent(int id) : DomainEvent
    {
        public int Id { get; } = id;
    }
}
