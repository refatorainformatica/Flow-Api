using Shared.Domain.Abstractions.Events;

namespace Services.Features.Financials.Contracts.Models.Events
{
    public class ContractRemovedEvent(int id) : DomainEvent
    {
        public int Id { get; } = id;
    }
}
