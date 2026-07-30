using Shared.Domain.Abstractions.Events;

namespace Services.Features.Peoples.Suppliers.Models.Events
{
    public class SupplierRemovedEvent(int id) : DomainEvent
    {
        public int Id { get; } = id;
    }
}
