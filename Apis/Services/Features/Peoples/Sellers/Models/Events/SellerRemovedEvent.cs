using Shared.Domain.Abstractions.Events;

namespace Services.Features.Peoples.Sellers.Models.Events
{
    public class SellerRemovedEvent(int id) : DomainEvent
    {
        public int Id { get; } = id;
    }
}
