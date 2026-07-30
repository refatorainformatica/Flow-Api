using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Services.Features.Financials.Invoices.Models;
using Services.Features.Financials.InvoiceTypes.Models.Events;
using Shared.Domain.Abstractions;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.InvoiceTypes.Models
{
    [Table("InvoiceTypes", Schema = "Financials")]
    public partial class InvoiceType : BaseEntity
    {
        public InvoiceType()
        {
            Invoices = [];
        }

        public InvoiceType(
            int id,
            string description,
            string picture,
            DateTime createdAt,
            string createdBy,
            DateTime editedAt,
            string editedBy
        )
        {
            Id = id;
            Description = description;
            Picture = picture;
            CreatedAt = createdAt;
            CreatedBy = createdBy;
            EditedAt = editedAt;
            EditedBy = editedBy;
        }

        public InvoiceType(int id, DateTime editedAt, string editedBy, DateTime deletedAt)
        {
            Id = id;
            EditedAt = editedAt;
            EditedBy = editedBy;
            DeletedAt = deletedAt;
        }

        [Required]
        [StringLength(256)]
        public string Description { get; set; }

        public string Picture { get; set; }

        [InverseProperty(nameof(Invoice.InvoiceType))]
        public virtual ICollection<Invoice> Invoices { get; set; }

        public override void OnCreatedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new InvoiceTypeCreatedEvent(Id));
        }

        public override void OnEditedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new InvoiceTypeEditedEvent(Id));
        }

        public override void OnRemovedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new InvoiceTypeRemovedEvent(Id));
        }
    }
}
