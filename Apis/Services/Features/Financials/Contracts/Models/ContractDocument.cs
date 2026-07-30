using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Services.Features.Financials.Contracts.Models.Events;
using Services.Features.Settings.DocumentTypes.Models;
using Shared.Domain.Abstractions;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.Contracts.Models
{
    [Table("ContractDocuments", Schema = "Financials")]
    public partial class ContractDocument : BaseEntity
    {
        public int DocumentTypeId { get; set; }

        public int ContractId { get; set; }

        [StringLength(256)]
        public string ExternalCode { get; set; }

        [StringLength(50)]
        public string EnrollmentCode { get; set; }

        [Column(TypeName = "date")]
        public DateTime? EnrollmentDate { get; set; }

        public string File { get; set; }

        public string Picture { get; set; }

        [ForeignKey(nameof(ContractId))]
        [InverseProperty(nameof(Contract.ContractDocuments))]
        public virtual Contract Contract { get; set; }

        [ForeignKey(nameof(DocumentTypeId))]
        [InverseProperty(nameof(DocumentType.ContractDocuments))]
        public virtual DocumentType DocumentType { get; set; }

        public override void OnCreatedEvent()
        {
            AddEvent(new ContractEditedEvent(Id));
        }

        public override void OnEditedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new ContractEditedEvent(Id));
        }

        public override void OnRemovedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new ContractRemovedEvent(Id));
        }
    }
}
