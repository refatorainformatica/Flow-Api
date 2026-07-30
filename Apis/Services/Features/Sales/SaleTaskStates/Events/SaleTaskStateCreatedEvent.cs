using Shared.Domain.Abstractions.Events;

namespace Services.Features.Sales.SaleTaskStates.Events
{
    public class SaleTaskStateCreatedEvent(int id) : DomainEvent
    {
        public int Id { get; } = id;
    }
}
