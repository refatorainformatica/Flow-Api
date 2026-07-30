using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Services.Features.Projects.Allocations;
using Services.Features.Projects.AllocationStates.Events;
using Shared.Domain.Abstractions;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Projects.AllocationStates
{
    [Table("AllocationStates", Schema = "Projects")]
    public partial class AllocationState : BaseEntity
    {
        public AllocationState()
        {
            Allocations = [];
        }

        [Required]
        [StringLength(256)]
        public string Description { get; set; }

        public string Picture { get; set; }

        [InverseProperty(nameof(Allocation.AllocationState))]
        public virtual ICollection<Allocation> Allocations { get; set; }

        public override void OnCreatedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new AllocationStateCreatedEvent(Id));
        }

        public override void OnEditedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new AllocationStateEditedEvent(Id));
        }

        public override void OnRemovedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new AllocationStateRemovedEvent(Id));
        }
    }
}
