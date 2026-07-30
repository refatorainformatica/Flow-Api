using Shared.Domain.Abstractions.Events;

namespace Services.Features.Peoples.ActivityBranchs.Models.Events
{
    public class ActivityBranchCreatedEvent(int id) : DomainEvent
    {
        public int Id { get; } = id;
    }
}
