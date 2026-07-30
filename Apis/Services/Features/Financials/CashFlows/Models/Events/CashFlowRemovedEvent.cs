using Shared.Domain.Abstractions.Events;

namespace Services.Features.Financials.CashFlows.Models.Events
{
    public class CashFlowRemovedEvent(int id) : DomainEvent
    {
        public int Id { get; } = id;
    }
}
