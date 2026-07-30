using Shared.Domain.Abstractions.Events;

namespace Services.Features.Financials.ContractTypes.Models.Events
{
    public class ContractTypeRemovedEvent(int id) : DomainEvent
    {
        public int Id { get; } = id;
    }
}
