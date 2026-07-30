using Shared.Domain.Abstractions.Events;

namespace Services.Features.Projects.Timesheets.Events
{
    public class TimesheetCreatedEvent(int id) : DomainEvent
    {
        public int Id { get; } = id;
    }
}
