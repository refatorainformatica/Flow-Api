using Shared.Domain.Abstractions.Events;

namespace Services.Features.Peoples.Talents.Models.Events
{
    public class TalentRemovedEvent(int id) : DomainEvent
    {
        public int Id { get; } = id;
    }
}
