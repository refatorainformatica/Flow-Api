using Shared.Domain.Abstractions.Events;

namespace Services.Features.Financials.Banks.Models.Events
{
    public class BankEditedEvent(int id) : DomainEvent
    {
        public int Id { get; } = id;
    }
}
