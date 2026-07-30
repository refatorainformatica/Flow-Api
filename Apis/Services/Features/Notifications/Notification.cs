using Services.Features.Notifications.Events;
using Shared.Domain.Abstractions;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Notifications
{
    public class Notification : BaseEntity
    {
        public string MessageId { get; private set; }
        public string Title { get; private set; }
        public string Body { get; private set; }
        public string Token { get; private set; }
        public string ImageUrl { get; private set; }
        public string Route { get; private set; }
        public string RouteArgs { get; private set; }
        public DateTime? OpeningAt { get; private set; }
        public string UserId { get; private set; }
        public bool IsScheduled { get; private set; }
        public DateTime? ScheduledAt { get; private set; }
        public DateTime? TriggeredAt { get; private set; }
        public string TriggeredStatus { get; private set; }
        public string TriggeredMessage { get; private set; }

        //public virtual User User { get; private set; }
        //public virtual User CreatedByUser { get; private set; }
        //public virtual User ModifiedByUser { get; private set; }

        public Notification(
            int id,
            string messageId,
            string userId,
            string title,
            string body,
            string token,
            string imageUrl,
            string route,
            string routeArgs,
            DateTime? openingAt,
            DateTime createdAt = default,
            string createdBy = "",
            DateTime editedAt = default,
            string editedBy = "",
            bool isScheduled = false,
            DateTime? scheduledAt = null,
            DateTime? triggeredAt = null,
            string triggeredStatus = "",
            string triggeredMessage = ""
        )
        {
            Id = id;
            MessageId = messageId;
            UserId = userId;
            Title = title;
            Body = body;
            Token = token;
            ImageUrl = imageUrl;
            Route = route;
            RouteArgs = routeArgs;
            OpeningAt = openingAt;
            CreatedAt = createdAt;
            CreatedBy = createdBy;
            EditedAt = editedAt;
            EditedBy = editedBy;
            IsScheduled = isScheduled;
            ScheduledAt = scheduledAt;
            TriggeredAt = triggeredAt;
            TriggeredStatus = triggeredStatus;
            TriggeredMessage = triggeredMessage;
        }

        public override void OnCreatedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new NotificationCreatedEvent(Id));
        }

        public override void OnEditedEvent()
        {
            if (Id == 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new NotificationEditedEvent(Id));
        }

        public override void OnRemovedEvent()
        {
            if (Id == 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new NotificationRemovedEvent(Id));
        }
    }
}
