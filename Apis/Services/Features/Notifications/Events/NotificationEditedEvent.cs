using Shared.Domain.Abstractions.Events;

namespace Services.Features.Notifications.Events
{
    public class NotificationEditedEvent(int id) : DomainEvent
    {
        public int Id { get; } = id;
    }
}
