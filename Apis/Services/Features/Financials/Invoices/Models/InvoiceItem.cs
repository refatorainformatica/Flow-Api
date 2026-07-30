using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Services.Features.Financials.Invoices.Models.Events;
using Shared.Domain.Abstractions;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.Invoices.Models
{
    [Table("InvoiceItems", Schema = "Financials")]
    public partial class InvoiceItem : BaseEntity
    {
        public int InvoiceId { get; set; }

        [Column(TypeName = "money")]
        public decimal Amount { get; set; }

        [Column(TypeName = "money")]
        public decimal Value { get; set; }

        [Required]
        [StringLength(256)]
        public string Description { get; set; }

        public string Observation { get; set; }

        public string Picture { get; set; }

        [ForeignKey(nameof(InvoiceId))]
        [InverseProperty(nameof(Invoice.InvoiceItems))]
        public virtual Invoice Invoice { get; set; }

        public override void OnCreatedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new InvoiceCreatedEvent(Id));
        }

        public override void OnEditedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new InvoiceEditedEvent(Id));
        }

        public override void OnRemovedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new InvoiceRemovedEvent(Id));
        }
    }
}
