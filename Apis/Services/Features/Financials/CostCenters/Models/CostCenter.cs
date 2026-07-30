using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Services.Features.Financials.CostCenters.Models.Events;
using Services.Features.Financials.Expenses.Models;
using Services.Features.Financials.Revenues.Models;
using Shared.Domain.Abstractions;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.CostCenters.Models
{
    [Table("CostCenters", Schema = "Financials")]
    public partial class CostCenter : BaseEntity
    {
        public CostCenter()
        {
            Expenses = [];
            Revenues = [];
        }

        public CostCenter(
            int id,
            string description,
            string picture,
            DateTime createdAt,
            string createdBy,
            DateTime editedAt,
            string editedBy
        )
        {
            Id = id;
            Description = description;
            Picture = picture;
            CreatedAt = createdAt;
            CreatedBy = createdBy;
            EditedAt = editedAt;
            EditedBy = editedBy;
        }

        public CostCenter(int id, DateTime editedAt, string editedBy, DateTime deletedAt)
        {
            Id = id;
            EditedAt = editedAt;
            EditedBy = editedBy;
            DeletedAt = deletedAt;
        }

        [StringLength(256)]
        public string Description { get; set; }

        public string Picture { get; set; }

        [InverseProperty(nameof(Expense.CostCenter))]
        public virtual ICollection<Expense> Expenses { get; set; }

        [InverseProperty(nameof(Revenue.CostCenter))]
        public virtual ICollection<Revenue> Revenues { get; set; }

        public override void OnCreatedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new CostCenterCreatedEvent(Id));
        }

        public override void OnEditedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new CostCenterEditedEvent(Id));
        }

        public override void OnRemovedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new CostCenterRemovedEvent(Id));
        }
    }
}
