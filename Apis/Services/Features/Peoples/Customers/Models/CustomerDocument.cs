using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Services.Features.Peoples.Customers.Models.Events;
using Services.Features.Settings.DocumentTypes.Models;
using Shared.Domain.Abstractions;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Peoples.Customers.Models
{
    [Table("CustomerDocuments", Schema = "Peoples")]
    public partial class CustomerDocument : BaseEntity
    {
        public int DocumentTypeId { get; set; }

        public int CustomerId { get; set; }

        [StringLength(256)]
        public string ExternalCode { get; set; }

        [StringLength(50)]
        public string EnrollmentCode { get; set; }

        [Column(TypeName = "date")]
        public DateTime? EnrollmentDate { get; set; }

        public string File { get; set; }
        public string Picture { get; set; }

        [ForeignKey(nameof(CustomerId))]
        [InverseProperty(nameof(Customer.Documents))]
        public virtual Customer Customer { get; set; }

        [ForeignKey(nameof(DocumentTypeId))]
        [InverseProperty(nameof(DocumentType.CustomerDocuments))]
        public virtual DocumentType DocumentType { get; set; }

        public override void OnCreatedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new CustomerCreatedEvent(Id));
        }

        public override void OnEditedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new CustomerEditedEvent(Id));
        }

        public override void OnRemovedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new CustomerRemovedEvent(Id));
        }

        public void AddDocumentType(DocumentType documentType)
        {
            DocumentType = documentType;
            DocumentTypeId = documentType == null ? 0 : documentType.Id;
        }
    }
}
