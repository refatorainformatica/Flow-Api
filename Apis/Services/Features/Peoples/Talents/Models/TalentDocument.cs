using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Services.Features.Peoples.Talents.Models.Events;
using Services.Features.Settings.DocumentTypes.Models;
using Shared.Domain.Abstractions;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Peoples.Talents.Models
{
    [Table("TalentDocuments", Schema = "Peoples")]
    public partial class TalentDocument : BaseEntity
    {
        public int DocumentTypeId { get; set; }

        public int TalentId { get; set; }

        [StringLength(256)]
        public string ExternalCode { get; set; }

        [StringLength(50)]
        public string EnrollmentCode { get; set; }

        [Column(TypeName = "date")]
        public DateTime? EnrollmentDate { get; set; }

        public string File { get; set; }
        public string Picture { get; set; }

        [ForeignKey(nameof(DocumentTypeId))]
        [InverseProperty(nameof(DocumentType.TalentDocuments))]
        public virtual DocumentType DocumentType { get; set; }

        [ForeignKey(nameof(TalentId))]
        [InverseProperty(nameof(Talent.Documents))]
        public virtual Talent Talent { get; set; }

        public override void OnCreatedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new TalentCreatedEvent(Id));
        }

        public override void OnEditedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new TalentEditedEvent(Id));
        }

        public override void OnRemovedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new TalentRemovedEvent(Id));
        }

        public void AddDocumentType(DocumentType documentType)
        {
            DocumentType = documentType;
            DocumentTypeId = documentType == null ? 0 : documentType.Id;
        }
    }
}
