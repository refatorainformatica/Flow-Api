using Shared.Domain.Abstractions.Events;

namespace Services.Features.Sales.SaleTaskTypes.Events
{
    public class SaleTaskTypeEditedEvent(int id) : DomainEvent
    {
        public int Id { get; } = id;
    }
}
