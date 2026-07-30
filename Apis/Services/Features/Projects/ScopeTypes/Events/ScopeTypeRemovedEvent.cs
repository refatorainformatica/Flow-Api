using Shared.Domain.Abstractions.Events;

namespace Services.Features.Projects.ScopeTypes.Events
{
    public class ScopeTypeRemovedEvent(int id) : DomainEvent
    {
        public int Id { get; } = id;
    }
}
