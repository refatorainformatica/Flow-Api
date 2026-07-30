using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Services.Features.Peoples.Suppliers.Models.Events;
using Services.Features.Settings.DocumentTypes.Models;
using Shared.Domain.Abstractions;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Peoples.Suppliers.Models
{
    [Table("SupplierDocuments", Schema = "Peoples")]
    public partial class SupplierDocument : BaseEntity
    {
        public int DocumentTypeId { get; set; }

        public int SupplierId { get; set; }

        [StringLength(256)]
        public string ExternalCode { get; set; }

        [StringLength(50)]
        public string EnrollmentCode { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? EnrollmentDate { get; set; }

        public string File { get; set; }
        public string Picture { get; set; }

        [ForeignKey(nameof(DocumentTypeId))]
        [InverseProperty(nameof(DocumentType.SupplierDocuments))]
        public virtual DocumentType DocumentType { get; set; }

        [ForeignKey(nameof(SupplierId))]
        [InverseProperty(nameof(Supplier.Documents))]
        public virtual Supplier Supplier { get; set; }

        public void AddDocumentType(DocumentType documentType)
        {
            DocumentType = documentType;
            DocumentTypeId = documentType == null ? 0 : documentType.Id;
        }

        public override void OnCreatedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new SupplierCreatedEvent(Id));
        }

        public override void OnEditedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new SupplierEditedEvent(Id));
        }

        public override void OnRemovedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new SupplierRemovedEvent(Id));
        }
    }
}
