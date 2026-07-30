using Shared.Domain.Abstractions.Events;

namespace Services.Features.Peoples.Sponsors.Models.Events
{
    public class SponsorEditedEvent(int id) : DomainEvent
    {
        public int Id { get; } = id;
    }
}
