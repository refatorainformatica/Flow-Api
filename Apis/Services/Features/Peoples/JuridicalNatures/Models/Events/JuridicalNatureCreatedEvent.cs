using Shared.Domain.Abstractions.Events;

namespace Services.Features.Peoples.JuridicalNatures.Models.Events
{
    public class JuridicalNatureCreatedEvent(int id) : DomainEvent
    {
        public int Id { get; } = id;
    }
}
