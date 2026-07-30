using System.Collections.Generic;

namespace Shared.Domain.Abstractions.Notifications
{
    public class NotificationMetadata
    {
        public string MessageId { get; private set; }
        public string Route { get; private set; }
        public Dictionary<string, object> RouteArgs { get; private set; }

        public NotificationMetadata(
            string messageId,
            string route,
            Dictionary<string, object> routeArgs = null
        )
        {
            MessageId = messageId;
            Route = route;
            RouteArgs = routeArgs;
        }
    }
}
