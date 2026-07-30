using Shared.Domain.Abstractions.Events;

namespace Services.Features.Financials.CurrencyTypes.Models.Events
{
    public class CurrencyTypeRemovedEvent(int id) : DomainEvent
    {
        public int Id { get; } = id;
    }
}
