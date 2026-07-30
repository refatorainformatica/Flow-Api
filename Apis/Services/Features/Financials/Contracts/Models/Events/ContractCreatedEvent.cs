using Shared.Domain.Abstractions.Events;

namespace Services.Features.Financials.Contracts.Models.Events
{
    public class ContractCreatedEvent(int id) : DomainEvent
    {
        public int Id { get; } = id;
    }
}
