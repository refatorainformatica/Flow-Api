using Shared.Domain.Abstractions.Events;

namespace Services.Features.Projects.TimesheetStates.Events
{
    public class TimesheetStateCreatedEvent(int id) : DomainEvent
    {
        public int Id { get; } = id;
    }
}
