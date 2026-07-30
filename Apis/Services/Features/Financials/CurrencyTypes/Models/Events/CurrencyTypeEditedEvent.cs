using Shared.Domain.Abstractions.Events;

namespace Services.Features.Financials.CurrencyTypes.Models.Events
{
    public class CurrencyTypeEditedEvent(int id) : DomainEvent
    {
        public int Id { get; } = id;
    }
}
