using Shared.Domain.Abstractions.Events;

namespace Services.Features.Peoples.SkillCategories.Models.Events
{
    public class SkillCategoryRemovedEvent(int id) : DomainEvent
    {
        public int Id { get; } = id;
    }
}
