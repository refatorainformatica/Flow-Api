using Shared.Domain.Abstractions.Events;

namespace Services.Features.Peoples.MaritalStates.Models.Events
{
    public class MaritalStateCreatedEvent(int id) : DomainEvent
    {
        public int Id { get; } = id;
    }
}
