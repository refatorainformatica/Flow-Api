using System.ComponentModel.DataAnnotations.Schema;
using Services.Features.Financials.Expenses.Models;
using Services.Features.Financials.Invoices.Models.Events;
using Services.Features.Financials.InvoiceStates.Models;
using Services.Features.Financials.InvoiceTypes.Models;
using Services.Features.Financials.Revenues.Models;
using Services.Features.Peoples.Suppliers.Models;
using Shared.Domain.Abstractions;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.Invoices.Models
{
    [Table("Invoices", Schema = "Financials")]
    public partial class Invoice : BaseEntity
    {
        public Invoice()
        {
            Expenses = [];
            InvoiceItems = [];
            Revenues = [];
        }

        public Invoice(
            int id,
            int supplierId,
            int invoiceTypeId,
            int invoiceStateId,
            string file,
            string picture,
            DateTime createdAt,
            string createdBy,
            DateTime editedAt,
            string editedBy,
            DateTime? deletedAt = null
        )
        {
            Id = id;
            SupplierId = supplierId;
            InvoiceTypeId = invoiceTypeId;
            InvoiceStateId = invoiceStateId;
            File = file;
            Picture = picture;
            CreatedAt = createdAt;
            CreatedBy = createdBy;
            EditedAt = editedAt;
            EditedBy = editedBy;
            DeletedAt = deletedAt;
        }

        public Invoice(int id, DateTime editedAt, string editedBy, DateTime deletedAt)
        {
            Id = id;
            EditedAt = editedAt;
            EditedBy = editedBy;
            DeletedAt = deletedAt;
        }

        public int SupplierId { get; set; }

        public int InvoiceTypeId { get; set; }

        public int InvoiceStateId { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime DateOfIssue { get; set; }

        public string File { get; set; }

        public string Picture { get; set; }

        [ForeignKey(nameof(InvoiceStateId))]
        [InverseProperty(nameof(InvoiceState.Invoices))]
        public virtual InvoiceState InvoiceState { get; set; }

        [ForeignKey(nameof(InvoiceTypeId))]
        [InverseProperty(nameof(InvoiceType.Invoices))]
        public virtual InvoiceType InvoiceType { get; set; }

        [ForeignKey(nameof(SupplierId))]
        [InverseProperty(nameof(Supplier.Invoices))]
        public virtual Supplier Supplier { get; set; }

        [InverseProperty(nameof(Expense.Invoice))]
        public virtual ICollection<Expense> Expenses { get; set; }

        [InverseProperty(nameof(InvoiceItem.Invoice))]
        public virtual ICollection<InvoiceItem> InvoiceItems { get; set; }

        [InverseProperty(nameof(Revenue.Invoice))]
        public virtual ICollection<Revenue> Revenues { get; set; }

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
