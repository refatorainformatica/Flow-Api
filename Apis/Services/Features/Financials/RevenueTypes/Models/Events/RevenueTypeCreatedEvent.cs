using Shared.Domain.Abstractions.Events;

namespace Services.Features.Financials.RevenueTypes.Models.Events
{
    public class RevenueTypeCreatedEvent(int id) : DomainEvent
    {
        public int Id { get; } = id;
    }
}
