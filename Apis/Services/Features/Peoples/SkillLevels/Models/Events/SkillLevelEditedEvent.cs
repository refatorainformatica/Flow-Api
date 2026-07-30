using Shared.Domain.Abstractions.Events;

namespace Services.Features.Peoples.SkillLevels.Models.Events
{
    public class SkillLevelEditedEvent(int id) : DomainEvent
    {
        public int Id { get; } = id;
    }
}
