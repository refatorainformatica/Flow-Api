using Shared.Domain.Abstractions.Events;

namespace Services.Features.Peoples.Talents.Models.Events
{
    public class TalentCreatedEvent(int id) : DomainEvent
    {
        public int Id { get; } = id;
    }
}
