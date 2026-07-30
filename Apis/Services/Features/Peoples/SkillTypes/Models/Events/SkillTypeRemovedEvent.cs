using Shared.Domain.Abstractions.Events;

namespace Services.Features.Peoples.SkillTypes.Models.Events
{
    public class SkillTypeRemovedEvent(int id) : DomainEvent
    {
        public int Id { get; } = id;
    }
}
