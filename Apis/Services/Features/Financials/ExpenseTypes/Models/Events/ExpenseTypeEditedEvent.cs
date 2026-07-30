using Shared.Domain.Abstractions.Events;

namespace Services.Features.Financials.ExpenseTypes.Models.Events
{
    public class ExpenseTypeEditedEvent(int id) : DomainEvent
    {
        public int Id { get; } = id;
    }
}
