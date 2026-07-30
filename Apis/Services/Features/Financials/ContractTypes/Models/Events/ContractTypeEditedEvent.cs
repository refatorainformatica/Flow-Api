using Shared.Domain.Abstractions.Events;

namespace Services.Features.Financials.ContractTypes.Models.Events
{
    public class ContractTypeEditedEvent(int id) : DomainEvent
    {
        public int Id { get; } = id;
    }
}
