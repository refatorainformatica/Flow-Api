using Shared.Domain.Abstractions.Events;

namespace Services.Features.Financials.ExpenseTypes.Models.Events
{
    public class ExpenseTypeCreatedEvent(int id) : DomainEvent
    {
        public int Id { get; } = id;
    }
}
