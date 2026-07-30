using Shared.Domain.Abstractions.Events;

namespace Services.Features.Sales.Opportunities.Events
{
    public class OpportunityCreatedEvent(int id) : DomainEvent
    {
        public int Id { get; } = id;
    }
}
