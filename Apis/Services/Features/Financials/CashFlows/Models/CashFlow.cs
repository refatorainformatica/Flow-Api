using System.ComponentModel.DataAnnotations.Schema;
using Services.Features.Financials.CashFlows.Models.Events;
using Services.Features.Financials.CurrencyTypes.Models;
using Services.Features.Financials.Expenses.Models;
using Services.Features.Financials.MovementTypes.Models;
using Services.Features.Financials.Revenues.Models;
using Shared.Domain.Abstractions;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.CashFlows.Models
{
    [Table("CashFlows", Schema = "Financials")]
    public partial class CashFlow : BaseEntity
    {
        public CashFlow() { }

        public CashFlow(
            int id,
            int yearExercise,
            int monthExercise,
            int movementTypeId,
            DateTime financialMovementDate,
            decimal financialMovementValue,
            decimal balanceValue,
            int? expenseId,
            int? revenueId,
            int? currencyTypeId,
            string picture,
            DateTime createdAt,
            string createdBy,
            DateTime editedAt,
            string editedBy,
            DateTime? deletedAt = null
        )
        {
            Id = id;
            YearExercise = yearExercise;
            MonthExercise = monthExercise;
            MovementTypeId = movementTypeId;
            FinancialMovementDate = financialMovementDate;
            FinancialMovementValue = financialMovementValue;
            BalanceValue = balanceValue;
            ExpenseId = expenseId;
            RevenueId = revenueId;
            CurrencyTypeId = currencyTypeId;
            Picture = picture;
            CreatedAt = createdAt;
            CreatedBy = createdBy;
            EditedAt = editedAt;
            EditedBy = editedBy;
            DeletedAt = deletedAt;
        }

        public CashFlow(int id, DateTime editedAt, string editedBy, DateTime deletedAt)
        {
            Id = id;
            EditedAt = editedAt;
            EditedBy = editedBy;
            DeletedAt = deletedAt;
        }

        public int YearExercise { get; set; }

        public int MonthExercise { get; set; }

        public int MovementTypeId { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime FinancialMovementDate { get; set; }

        [Column(TypeName = "money")]
        public decimal FinancialMovementValue { get; set; }

        [Column(TypeName = "money")]
        public decimal BalanceValue { get; set; }

        public int? ExpenseId { get; set; }

        public int? RevenueId { get; set; }

        public int? CurrencyTypeId { get; set; }

        public string Picture { get; set; }

        [ForeignKey(nameof(CurrencyTypeId))]
        [InverseProperty(nameof(CurrencyType.CashFlows))]
        public virtual CurrencyType CurrencyType { get; set; }

        [ForeignKey(nameof(ExpenseId))]
        [InverseProperty(nameof(Expense.CashFlows))]
        public virtual Expense Expense { get; set; }

        [ForeignKey(nameof(MovementTypeId))]
        [InverseProperty(nameof(MovementType.CashFlows))]
        public virtual MovementType MovementType { get; set; }

        [ForeignKey(nameof(RevenueId))]
        [InverseProperty(nameof(Revenue.CashFlows))]
        public virtual Revenue Revenue { get; set; }

        public override void OnCreatedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new CashFlowCreatedEvent(Id));
        }

        public override void OnEditedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new CashFlowEditedEvent(Id));
        }

        public override void OnRemovedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new CashFlowRemovedEvent(Id));
        }
    }
}
