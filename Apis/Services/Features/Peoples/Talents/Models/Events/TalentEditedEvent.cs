using Shared.Domain.Abstractions.Events;

namespace Services.Features.Peoples.Talents.Models.Events
{
    public class TalentEditedEvent(int id) : DomainEvent
    {
        public int Id { get; } = id;
    }
}
