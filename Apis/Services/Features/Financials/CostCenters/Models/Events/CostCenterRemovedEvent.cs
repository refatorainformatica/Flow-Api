using Shared.Domain.Abstractions.Events;

namespace Services.Features.Financials.CostCenters.Models.Events
{
    public class CostCenterRemovedEvent(int id) : DomainEvent
    {
        public int Id { get; } = id;
    }
}
