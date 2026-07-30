using Shared.Domain.Abstractions.Events;

namespace Services.Features.Projects.TimesheetStates.Events
{
    public class TimesheetStateRemovedEvent(int id) : DomainEvent
    {
        public int Id { get; } = id;
    }
}
