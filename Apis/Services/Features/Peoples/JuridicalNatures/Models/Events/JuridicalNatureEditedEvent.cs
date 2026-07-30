using Shared.Domain.Abstractions.Events;

namespace Services.Features.Peoples.JuridicalNatures.Models.Events
{
    public class JuridicalNatureEditedEvent(int id) : DomainEvent
    {
        public int Id { get; } = id;
    }
}
