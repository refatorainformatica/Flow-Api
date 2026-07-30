using Shared.Domain.Abstractions.Events;

namespace Services.Features.Financials.MovementTypes.Models.Events
{
    public class MovementTypeEditedEvent(int id) : DomainEvent
    {
        public int Id { get; } = id;
    }
}
