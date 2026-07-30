using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Services.Features.Peoples.Sponsors.Models.Events;
using Services.Features.Settings.DocumentTypes.Models;
using Shared.Domain.Abstractions;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Peoples.Sponsors.Models
{
    [Table("SponsorDocuments", Schema = "Peoples")]
    public partial class SponsorDocument : BaseEntity
    {
        public int DocumentTypeId { get; set; }

        public int SponsorId { get; set; }

        [StringLength(256)]
        public string ExternalCode { get; set; }

        [StringLength(50)]
        public string EnrollmentCode { get; set; }

        [Column(TypeName = "date")]
        public DateTime? EnrollmentDate { get; set; }

        public string File { get; set; }
        public string Picture { get; set; }

        [ForeignKey(nameof(DocumentTypeId))]
        [InverseProperty(nameof(DocumentType.SponsorDocuments))]
        public virtual DocumentType DocumentType { get; set; }

        [JsonIgnore]
        [ForeignKey(nameof(SponsorId))]
        [InverseProperty(nameof(Sponsor.Documents))]
        public virtual Sponsor Sponsor { get; set; }

        public override void OnCreatedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new SponsorCreatedEvent(Id));
        }

        public override void OnEditedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new SponsorEditedEvent(Id));
        }

        public override void OnRemovedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new SponsorRemovedEvent(Id));
        }

        public void AddDocumentType(DocumentType documentType)
        {
            DocumentType = documentType;
            DocumentTypeId = documentType == null ? 0 : documentType.Id;
        }
    }
}
