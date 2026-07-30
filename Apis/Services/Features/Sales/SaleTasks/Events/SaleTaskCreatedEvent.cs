using Shared.Domain.Abstractions.Events;

namespace Services.Features.Sales.SaleTasks.Events
{
    public class SaleTaskCreatedEvent(int id) : DomainEvent
    {
        public int Id { get; } = id;
    }
}
