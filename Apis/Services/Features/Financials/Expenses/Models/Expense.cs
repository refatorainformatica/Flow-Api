using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Services.Features.Financials.CashFlows.Models;
using Services.Features.Financials.CostCenters.Models;
using Services.Features.Financials.Expenses.Models.Events;
using Services.Features.Financials.ExpenseTypes.Models;
using Services.Features.Financials.Invoices.Models;
using Services.Features.Financials.PaymentStates.Models;
using Shared.Domain.Abstractions;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.Expenses.Models
{
    [Table("Expenses", Schema = "Financials")]
    public partial class Expense : BaseEntity
    {
        public Expense()
        {
            CashFlows = [];
        }

        public Expense(
            int id,
            int invoiceId,
            DateTime dateOfIssue,
            DateTime dateOfDue,
            DateTime dateOfPayment,
            int installmentNumber,
            int totalNumberOfInstallments,
            decimal paymentValue,
            decimal paymentDiscountValue,
            decimal totalPaymentValue,
            string barCode,
            string observation,
            int? costCenterId,
            int? paymentStateId,
            int? expenseTypeId,
            string picture,
            DateTime createdAt,
            string createdBy,
            DateTime editedAt,
            string editedBy
        )
        {
            Id = id;
            InvoiceId = invoiceId;
            DateOfIssue = dateOfIssue;
            DateOfDue = dateOfDue;
            DateOfPayment = dateOfPayment;
            InstallmentNumber = installmentNumber;
            TotalNumberOfInstallments = totalNumberOfInstallments;
            PaymentValue = paymentValue;
            PaymentDiscountValue = paymentDiscountValue;
            TotalPaymentValue = totalPaymentValue;
            BarCode = barCode;
            Observation = observation;
            CostCenterId = costCenterId;
            PaymentStateId = paymentStateId;
            ExpenseTypeId = expenseTypeId;
            Picture = picture;
            CreatedAt = createdAt;
            CreatedBy = createdBy;
            EditedAt = editedAt;
            EditedBy = editedBy;
        }

        public Expense(int id, DateTime editedAt, string editedBy, DateTime deletedAt)
        {
            Id = id;
            EditedAt = editedAt;
            EditedBy = editedBy;
            DeletedAt = deletedAt;
        }

        public int InvoiceId { get; set; }

        [Column(TypeName = "date")]
        public DateTime DateOfIssue { get; set; }

        [Column(TypeName = "date")]
        public DateTime DateOfDue { get; set; }

        [Column(TypeName = "date")]
        public DateTime DateOfPayment { get; set; }

        public int InstallmentNumber { get; set; }

        public int TotalNumberOfInstallments { get; set; }

        [Column(TypeName = "money")]
        public decimal PaymentValue { get; set; }

        [Column(TypeName = "money")]
        public decimal PaymentDiscountValue { get; set; }

        [Column(TypeName = "money")]
        public decimal TotalPaymentValue { get; set; }

        [StringLength(255)]
        public string BarCode { get; set; }

        public string Observation { get; set; }

        public int? CostCenterId { get; set; }

        public int? PaymentStateId { get; set; }

        public int? ExpenseTypeId { get; set; }

        public string Picture { get; set; }

        [ForeignKey(nameof(PaymentStateId))]
        [InverseProperty(nameof(PaymentState.Expenses))]
        public virtual PaymentState PaymentState { get; set; }

        [ForeignKey(nameof(CostCenterId))]
        [InverseProperty(nameof(CostCenter.Expenses))]
        public virtual CostCenter CostCenter { get; set; }

        [ForeignKey(nameof(ExpenseTypeId))]
        [InverseProperty(nameof(ExpenseType.Expenses))]
        public virtual ExpenseType ExpenseType { get; set; }

        [ForeignKey(nameof(InvoiceId))]
        [InverseProperty(nameof(Invoice.Expenses))]
        public virtual Invoice Invoice { get; set; }

        [InverseProperty(nameof(CashFlow.Expense))]
        public virtual ICollection<CashFlow> CashFlows { get; set; }

        public override void OnCreatedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new ExpenseCreatedEvent(Id));
        }

        public override void OnEditedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new ExpenseEditedEvent(Id));
        }

        public override void OnRemovedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new ExpenseRemovedEvent(Id));
        }
    }
}
