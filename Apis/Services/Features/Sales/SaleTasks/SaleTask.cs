using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Services.Features.Sales.Opportunities;
using Services.Features.Sales.SaleTasks.Events;
using Services.Features.Sales.SaleTaskStates;
using Services.Features.Sales.SaleTaskTypes;
using Shared.Domain.Abstractions;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Sales.SaleTasks
{
    [Table("SaleTasks", Schema = "Sales")]
    public partial class SaleTask : BaseEntity
    {
        [Required]
        [StringLength(256)]
        public string Title { get; set; }

        public int OpportunityId { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime DueDate { get; set; }

        public int SaleTaskTypeId { get; set; }

        public int? SaleTaskStateId { get; set; }

        [ForeignKey(nameof(OpportunityId))]
        [InverseProperty(nameof(Opportunity.Tasks))]
        public virtual Opportunity Opportunity { get; set; }

        [ForeignKey(nameof(SaleTaskStateId))]
        [InverseProperty(nameof(SaleTaskStates.SaleTaskState.Tasks))]
        public virtual SaleTaskState SaleTaskState { get; set; }

        [ForeignKey(nameof(SaleTaskTypeId))]
        [InverseProperty(nameof(SaleTaskType.Tasks))]
        public virtual SaleTaskType TaskType { get; set; }

        public override void OnCreatedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new SaleTaskCreatedEvent(Id));
        }

        public override void OnEditedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new SaleTaskEditedEvent(Id));
        }

        public override void OnRemovedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new SaleTaskRemovedEvent(Id));
        }
    }
}
