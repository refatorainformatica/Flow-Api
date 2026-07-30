using Shared.Domain.Abstractions.Events;

namespace Services.Features.Financials.Revenues.Models.Events
{
    public class RevenueEditedEvent(int id) : DomainEvent
    {
        public int Id { get; } = id;
    }
}
