using Shared.Domain.Abstractions.Events;

namespace Services.Features.Peoples.Sponsors.Models.Events
{
    public class SponsorRemovedEvent(int id) : DomainEvent
    {
        public int Id { get; } = id;
    }
}
