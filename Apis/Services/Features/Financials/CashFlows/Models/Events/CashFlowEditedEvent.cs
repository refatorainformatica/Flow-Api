using Shared.Domain.Abstractions.Events;

namespace Services.Features.Financials.CashFlows.Models.Events
{
    public class CashFlowEditedEvent(int id) : DomainEvent
    {
        public int Id { get; } = id;
    }
}
