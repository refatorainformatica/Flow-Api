using Shared.Domain.Abstractions.Events;

namespace Services.Features.Financials.Expenses.Models.Events
{
    public class ExpenseRemovedEvent(int id) : DomainEvent
    {
        public int Id { get; } = id;
    }
}
