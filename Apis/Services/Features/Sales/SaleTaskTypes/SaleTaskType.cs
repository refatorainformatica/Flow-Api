using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Services.Features.Sales.SaleTasks;
using Services.Features.Sales.SaleTaskTypes.Events;
using Shared.Domain.Abstractions;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Sales.SaleTaskTypes
{
    [Table("SaleTaskTypes", Schema = "Sales")]
    public partial class SaleTaskType : BaseEntity
    {
        public SaleTaskType()
        {
            Tasks = [];
        }

        [Required]
        [StringLength(256)]
        public string Name { get; set; }

        [InverseProperty(nameof(SaleTask.TaskType))]
        public virtual ICollection<SaleTask> Tasks { get; set; }

        public override void OnCreatedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new SaleTaskTypeCreatedEvent(Id));
        }

        public override void OnEditedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new SaleTaskTypeEditedEvent(Id));
        }

        public override void OnRemovedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new SaleTaskTypeRemovedEvent(Id));
        }
    }
}
