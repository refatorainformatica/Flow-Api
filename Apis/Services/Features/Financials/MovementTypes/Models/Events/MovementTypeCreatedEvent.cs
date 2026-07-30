using Shared.Domain.Abstractions.Events;

namespace Services.Features.Financials.MovementTypes.Models.Events
{
    public class MovementTypeCreatedEvent(int id) : DomainEvent
    {
        public int Id { get; } = id;
    }
}
