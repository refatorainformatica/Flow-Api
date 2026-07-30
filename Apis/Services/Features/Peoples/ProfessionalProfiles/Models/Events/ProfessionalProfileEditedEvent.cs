using Shared.Domain.Abstractions.Events;

namespace Services.Features.Peoples.ProfessionalProfiles.Models.Events
{
    public class ProfessionalProfileEditedEvent(int id) : DomainEvent
    {
        public int Id { get; } = id;
    }
}
