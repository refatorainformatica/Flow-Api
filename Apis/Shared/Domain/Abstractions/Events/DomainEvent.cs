namespace Shared.Domain.Abstractions.Events
{
    public abstract class DomainEvent : IEvent
    {
        public System.DateTime TriggeredOn { get; protected set; } = System.DateTime.UtcNow;
    }
}
