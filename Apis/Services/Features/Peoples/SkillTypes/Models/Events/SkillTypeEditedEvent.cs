using Shared.Domain.Abstractions.Events;

namespace Services.Features.Peoples.SkillTypes.Models.Events
{
    public class SkillTypeEditedEvent(int id) : DomainEvent
    {
        public int Id { get; } = id;
    }
}
