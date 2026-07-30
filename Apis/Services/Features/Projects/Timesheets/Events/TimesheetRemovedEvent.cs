using Shared.Domain.Abstractions.Events;

namespace Services.Features.Projects.Timesheets.Events
{
    public class TimesheetRemovedEvent(int id) : DomainEvent
    {
        public int Id { get; } = id;
    }
}
