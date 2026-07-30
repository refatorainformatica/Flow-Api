using Shared.Domain.Abstractions.Events;

namespace Services.Features.Peoples.Careers.Models.Events
{
    public class CareerEditedEvent(int id) : DomainEvent
    {
        public int Id { get; } = id;
    }
}
