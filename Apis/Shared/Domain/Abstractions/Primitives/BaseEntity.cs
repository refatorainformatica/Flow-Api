using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Shared.Domain.Abstractions.Events;

namespace Shared.Domain.Abstractions.Primitives
{
    public abstract class BaseEntity : AuditableBaseEntity
    {
        private IList<DomainEvent> DomainEvents { get; } = [];

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public override bool Equals(object obj)
        {
            var compareTo = obj as BaseEntity;

            if (ReferenceEquals(this, compareTo))
                return true;
            if (compareTo is null)
                return false;

            return Id.Equals(compareTo.Id);
        }

        public override int GetHashCode()
        {
            return GetType().GetHashCode() * 907 + Id.GetHashCode();
        }

        public override string ToString()
        {
            return GetType().Name + " [Id=" + Id + "]";
        }

        public void AddEvent(DomainEvent domainEvent)
        {
            DomainEvents.Add(domainEvent);
        }

        public IList<DomainEvent> GetEvents()
        {
            return DomainEvents;
        }

        public void RemoveEvents()
        {
            DomainEvents.Clear();
        }

        public void SetCreateAuditoryData(string user)
        {
            CreatedBy = user;
            CreatedAt = System.DateTime.Now;
            EditedBy = user;
            EditedAt = System.DateTime.Now;
        }

        public void SetEditAuditoryData(string user)
        {
            EditedBy = user;
            EditedAt = System.DateTime.Now;
        }

        public void SetDeleteAuditoryData(string user)
        {
            EditedBy = user;
            EditedAt = System.DateTime.Now;
            DeletedAt = System.DateTime.Now;
        }

        public abstract void OnCreatedEvent();

        public abstract void OnEditedEvent();

        public abstract void OnRemovedEvent();
    }
}
