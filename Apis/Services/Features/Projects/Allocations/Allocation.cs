using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Services.Features.Peoples.ProfessionalProfiles.Models;
using Services.Features.Peoples.Suppliers.Models;
using Services.Features.Projects.Allocations.Events;
using Services.Features.Projects.AllocationStates;
using Services.Features.Projects.Projects.Models;
using Shared.Domain.Abstractions;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Projects.Allocations
{
    [Table("Allocations", Schema = "Projects")]
    public partial class Allocation : BaseEntity
    {
        public int ProjectId { get; set; }

        public int SupplierId { get; set; }

        [StringLength(256)]
        public string Description { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime StartDate { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? EndDate { get; set; }

        public int ProfessionalProfileId { get; set; }

        [Column(TypeName = "money")]
        public decimal HourlyValue { get; set; }

        [Column(TypeName = "money")]
        public decimal OvertimeValue { get; set; }

        [Column(TypeName = "money")]
        public decimal AdditionalHourlyValue { get; set; }

        public int AllocationStateId { get; set; }

        public string Picture { get; set; }

        [ForeignKey(nameof(AllocationStateId))]
        [InverseProperty(nameof(AllocationState.Allocations))]
        public virtual AllocationState AllocationState { get; set; }

        [ForeignKey(nameof(ProfessionalProfileId))]
        [InverseProperty(nameof(ProfessionalProfile.Allocations))]
        public virtual ProfessionalProfile ProfessionalProfile { get; set; }

        [ForeignKey(nameof(ProjectId))]
        [InverseProperty(nameof(Project.Allocations))]
        public virtual Project Project { get; set; }

        [ForeignKey(nameof(SupplierId))]
        [InverseProperty(nameof(Supplier.ProjectAllocations))]
        public virtual Supplier Supplier { get; set; }

        public override void OnCreatedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new AllocationCreatedEvent(Id));
        }

        public override void OnEditedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new AllocationEditedEvent(Id));
        }

        public override void OnRemovedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new AllocationRemovedEvent(Id));
        }
    }
}
