using Shared.Domain.Abstractions.Events;

namespace Services.Features.Sales.OpportunityStates.Events
{
    public class OpportunityStateCreatedEvent(int id) : DomainEvent
    {
        public int Id { get; } = id;
    }
}
