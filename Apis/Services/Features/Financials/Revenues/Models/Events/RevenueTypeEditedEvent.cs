using Shared.Domain.Abstractions.Events;

namespace Services.Features.Financials.Revenues.Models.Events
{
    public class RevenueTypeEditedEvent(int id) : DomainEvent
    {
        public int Id { get; } = id;
    }
}
