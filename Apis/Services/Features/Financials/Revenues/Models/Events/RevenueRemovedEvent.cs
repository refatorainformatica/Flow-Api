using Shared.Domain.Abstractions.Events;

namespace Services.Features.Financials.Revenues.Models.Events
{
    public class RevenueRemovedEvent(int id) : DomainEvent
    {
        public int Id { get; } = id;
    }
}
