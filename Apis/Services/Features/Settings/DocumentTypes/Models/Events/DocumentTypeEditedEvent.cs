using Shared.Domain.Abstractions.Events;

namespace Services.Features.Settings.DocumentTypes.Models.Events
{
    public class DocumentTypeEditedEvent(int id) : DomainEvent
    {
        public int Id { get; } = id;
    }
}
