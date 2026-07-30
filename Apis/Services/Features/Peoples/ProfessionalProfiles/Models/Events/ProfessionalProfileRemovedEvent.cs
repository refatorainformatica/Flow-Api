using Shared.Domain.Abstractions.Events;

namespace Services.Features.Peoples.ProfessionalProfiles.Models.Events
{
    public class ProfessionalProfileRemovedEvent(int id) : DomainEvent
    {
        public int Id { get; } = id;
    }
}
