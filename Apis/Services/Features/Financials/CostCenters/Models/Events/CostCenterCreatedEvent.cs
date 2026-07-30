using Shared.Domain.Abstractions.Events;

namespace Services.Features.Financials.CostCenters.Models.Events
{
    public class CostCenterCreatedEvent(int id) : DomainEvent
    {
        public int Id { get; } = id;
    }
}
