using Shared.Domain.Abstractions.Events;

namespace Services.Features.Projects.ScopeTypes.Events
{
    public class ScopeTypeCreatedEvent(int id) : DomainEvent
    {
        public int Id { get; } = id;
    }
}
