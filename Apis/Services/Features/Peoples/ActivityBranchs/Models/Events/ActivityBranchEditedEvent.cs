using Shared.Domain.Abstractions.Events;

namespace Services.Features.Peoples.ActivityBranchs.Models.Events
{
    public class ActivityBranchEditedEvent(int id) : DomainEvent
    {
        public int Id { get; } = id;
    }
}
