using Shared.Domain.Abstractions.Events;

namespace Services.Features.Peoples.EducationLevels.Models.Events
{
    public class EducationLevelCreatedEvent(int id) : DomainEvent
    {
        public int Id { get; } = id;
    }
}
