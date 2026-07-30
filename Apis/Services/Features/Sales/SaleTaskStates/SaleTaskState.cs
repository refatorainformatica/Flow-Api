using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Services.Features.Sales.SaleTasks;
using Services.Features.Sales.SaleTaskStates.Events;
using Shared.Domain.Abstractions;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Sales.SaleTaskStates
{
    [Table("SaleTaskStates", Schema = "Sales")]
    public partial class SaleTaskState : BaseEntity
    {
        public SaleTaskState()
        {
            Tasks = [];
        }

        [Required]
        [StringLength(256)]
        public string Name { get; set; }

        [InverseProperty(nameof(SaleTask.SaleTaskState))]
        public virtual ICollection<SaleTask> Tasks { get; set; }

        public override void OnCreatedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new SaleTaskStateCreatedEvent(Id));
        }

        public override void OnEditedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new SaleTaskStateEditedEvent(Id));
        }

        public override void OnRemovedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new SaleTaskStateRemovedEvent(Id));
        }
    }
}
