using Shared.Domain.Abstractions.Events;

namespace Services.Features.Peoples.JuridicalNatures.Models.Events
{
    public class JuridicalNatureRemovedEvent(int id) : DomainEvent
    {
        public int Id { get; } = id;
    }
}
