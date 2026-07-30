using Shared.Domain.Abstractions.Events;

namespace Services.Features.Sales.SaleTaskStates.Events
{
    public class SaleTaskStateEditedEvent(int id) : DomainEvent
    {
        public int Id { get; } = id;
    }
}
