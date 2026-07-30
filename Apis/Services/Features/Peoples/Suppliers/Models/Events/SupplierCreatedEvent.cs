using Shared.Domain.Abstractions.Events;

namespace Services.Features.Peoples.Suppliers.Models.Events
{
    public class SupplierCreatedEvent(int id) : DomainEvent
    {
        public int Id { get; } = id;
    }
}
