using Shared.Domain.Abstractions.Events;

namespace Services.Features.Financials.ContractStates.Models.Events
{
    public class ContractStateEditedEvent(int id) : DomainEvent
    {
        public int Id { get; } = id;
    }
}
