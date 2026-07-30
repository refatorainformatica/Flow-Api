using Shared.Domain.Abstractions.Events;

namespace Services.Features.Financials.Expenses.Models.Events
{
    public class ExpenseCreatedEvent(int id) : DomainEvent
    {
        public int Id { get; } = id;
    }
}
