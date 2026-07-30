using Shared.Domain.Abstractions.Events;

namespace Services.Features.Financials.ExpenseTypes.Models.Events
{
    public class ExpenseTypeRemovedEvent(int id) : DomainEvent
    {
        public int Id { get; } = id;
    }
}
