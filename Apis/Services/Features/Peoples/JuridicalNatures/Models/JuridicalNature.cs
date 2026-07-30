using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Services.Features.Peoples.JuridicalNatures.Models.Events;
using Services.Features.Peoples.Suppliers.Models;
using Shared.Domain.Abstractions;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Peoples.JuridicalNatures.Models
{
    [Table("JuridicalNatures", Schema = "Peoples")]
    public partial class JuridicalNature : BaseEntity
    {
        public JuridicalNature()
        {
            Suppliers = [];
        }

        public JuridicalNature(
            int id,
            string externalCode,
            string description,
            string picture,
            DateTime createdAt,
            string createdBy,
            DateTime editedAt,
            string editedBy
        )
        {
            Id = id;
            ExternalCode = externalCode;
            Description = description;
            Picture = picture;
            CreatedAt = createdAt;
            CreatedBy = createdBy;
            EditedAt = editedAt;
            EditedBy = editedBy;
        }

        public JuridicalNature(int id, DateTime editedAt, string editedBy, DateTime deletedAt)
        {
            Id = id;
            EditedAt = editedAt;
            EditedBy = editedBy;
            DeletedAt = deletedAt;
        }

        [StringLength(256)]
        public string ExternalCode { get; set; }

        [StringLength(256)]
        public string Description { get; set; }

        public string Picture { get; set; }

        [InverseProperty(nameof(Supplier.JuridicalNature))]
        public virtual ICollection<Supplier> Suppliers { get; set; }

        public override void OnCreatedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new JuridicalNatureCreatedEvent(Id));
        }

        public override void OnEditedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new JuridicalNatureEditedEvent(Id));
        }

        public override void OnRemovedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new JuridicalNatureRemovedEvent(Id));
        }
    }
}
