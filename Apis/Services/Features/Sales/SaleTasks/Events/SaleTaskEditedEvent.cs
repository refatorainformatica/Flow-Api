using Shared.Domain.Abstractions.Events;

namespace Services.Features.Sales.SaleTasks.Events
{
    public class SaleTaskEditedEvent(int id) : DomainEvent
    {
        public int Id { get; } = id;
    }
}
