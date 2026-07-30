using Shared.Domain.Abstractions.Events;

namespace Services.Features.Peoples.SkillStates.Models.Events
{
    public class SkillStateRemovedEvent(int id) : DomainEvent
    {
        public int Id { get; } = id;
    }
}
