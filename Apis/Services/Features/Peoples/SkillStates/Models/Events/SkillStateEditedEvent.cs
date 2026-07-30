using Shared.Domain.Abstractions.Events;

namespace Services.Features.Peoples.SkillStates.Models.Events
{
    public class SkillStateEditedEvent(int id) : DomainEvent
    {
        public int Id { get; } = id;
    }
}
