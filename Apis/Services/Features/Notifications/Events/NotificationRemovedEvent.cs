using Shared.Domain.Abstractions.Events;

namespace Services.Features.Notifications.Events
{
    public class NotificationRemovedEvent(int id) : DomainEvent
    {
        public int Id { get; } = id;
    }
}
