using Shared.Domain.Abstractions.Events;

namespace Services.Features.Sales.OpportunityStates.Events
{
    public class OpportunityStateRemovedEvent(int id) : DomainEvent
    {
        public int Id { get; } = id;
    }
}
