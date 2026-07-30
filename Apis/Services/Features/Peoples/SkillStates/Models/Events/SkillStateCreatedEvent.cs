using Shared.Domain.Abstractions.Events;

namespace Services.Features.Peoples.SkillStates.Models.Events
{
    public class SkillStateCreatedEvent(int id) : DomainEvent
    {
        public int Id { get; } = id;
    }
}
