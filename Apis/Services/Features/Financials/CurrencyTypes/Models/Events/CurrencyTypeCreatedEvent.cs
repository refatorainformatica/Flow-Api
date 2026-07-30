using Shared.Domain.Abstractions.Events;

namespace Services.Features.Financials.CurrencyTypes.Models.Events
{
    public class CurrencyTypeCreatedEvent(int id) : DomainEvent
    {
        public int Id { get; } = id;
    }
}
