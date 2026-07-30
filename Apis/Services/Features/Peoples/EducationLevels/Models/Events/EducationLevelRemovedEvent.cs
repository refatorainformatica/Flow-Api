using Shared.Domain.Abstractions.Events;

namespace Services.Features.Peoples.EducationLevels.Models.Events
{
    public class EducationLevelRemovedEvent(int id) : DomainEvent
    {
        public int Id { get; } = id;
    }
}
