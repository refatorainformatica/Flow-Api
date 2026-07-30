using Shared.Domain.Abstractions.Events;

namespace Services.Features.Financials.Banks.Models.Events
{
    public class BankCreatedEvent(int id) : DomainEvent
    {
        public int Id { get; } = id;
    }
}
