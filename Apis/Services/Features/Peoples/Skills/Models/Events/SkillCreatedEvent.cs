using Shared.Domain.Abstractions.Events;

namespace Services.Features.Peoples.Skills.Models.Events
{
    public class SkillCreatedEvent(int id) : DomainEvent
    {
        public int Id { get; } = id;
    }
}
