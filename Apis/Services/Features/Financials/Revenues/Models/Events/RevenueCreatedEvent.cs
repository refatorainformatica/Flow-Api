using Shared.Domain.Abstractions.Events;

namespace Services.Features.Financials.Revenues.Models.Events
{
    public class RevenueCreatedEvent(int id) : DomainEvent
    {
        public int Id { get; } = id;
    }
}
