using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Services.Features.Peoples.Sellers.Models.Events;
using Services.Features.Settings.DocumentTypes.Models;
using Shared.Domain.Abstractions;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Peoples.Sellers.Models
{
    [Table("SellerDocuments", Schema = "Peoples")]
    public partial class SellerDocument : BaseEntity
    {
        public int DocumentTypeId { get; set; }

        public int SellerId { get; set; }

        [StringLength(256)]
        public string ExternalCode { get; set; }

        [StringLength(50)]
        public string EnrollmentCode { get; set; }

        [Column(TypeName = "date")]
        public DateTime? EnrollmentDate { get; set; }

        public string File { get; set; }
        public string Picture { get; set; }

        [ForeignKey(nameof(DocumentTypeId))]
        [InverseProperty(nameof(DocumentType.SellerDocuments))]
        public virtual DocumentType DocumentType { get; set; }

        [ForeignKey(nameof(SellerId))]
        [InverseProperty(nameof(Seller.Documents))]
        public virtual Seller Seller { get; set; }

        public override void OnCreatedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new SellerCreatedEvent(Id));
        }

        public override void OnEditedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new SellerEditedEvent(Id));
        }

        public override void OnRemovedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new SellerRemovedEvent(Id));
        }

        public void AddDocumentType(DocumentType documentType)
        {
            DocumentType = documentType;
            DocumentTypeId = documentType == null ? 0 : documentType.Id;
        }
    }
}
