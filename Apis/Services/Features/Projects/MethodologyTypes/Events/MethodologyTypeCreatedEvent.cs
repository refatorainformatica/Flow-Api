using Shared.Domain.Abstractions.Events;

namespace Services.Features.Projects.MethodologyTypes.Events
{
    public class MethodologyTypeCreatedEvent(int id) : DomainEvent
    {
        public int Id { get; } = id;
    }
}
