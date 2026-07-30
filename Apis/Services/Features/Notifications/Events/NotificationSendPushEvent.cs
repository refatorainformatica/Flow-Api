using Shared.Domain.Abstractions.Events;
using Shared.Domain.Abstractions.Notifications;

namespace Services.Features.Notifications.Events
{
    public class NotificationSendPushEvent(
        string title,
        string body,
        string token,
        string imageUrl = "",
        NotificationMetadata notificationRouteConfig = default
    ) : DomainEvent
    {
        public string Title { get; } = title;
        public string Body { get; } = body;
        public string Token { get; } = token;
        public string ImageUrl { get; } = imageUrl;
        public NotificationMetadata NotificationMetadata { get; } = notificationRouteConfig;
    }
}
