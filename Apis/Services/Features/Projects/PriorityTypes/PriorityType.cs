using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Services.Features.Projects.PriorityTypes.Events;
using Services.Features.Projects.ProjectTasks;
using Shared.Domain.Abstractions;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Projects.PriorityTypes
{
    [Table("PriorityTypes", Schema = "Projects")]
    public partial class PriorityType : BaseEntity
    {
        public PriorityType()
        {
            ProjectTasks = [];
        }

        [StringLength(256)]
        public string Description { get; set; }

        public string Picture { get; set; }

        [InverseProperty(nameof(ProjectTask.PriorityType))]
        public virtual ICollection<ProjectTask> ProjectTasks { get; set; }

        public override void OnCreatedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new PriorityTypeCreatedEvent(Id));
        }

        public override void OnEditedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new PriorityTypeEditedEvent(Id));
        }

        public override void OnRemovedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new PriorityTypeRemovedEvent(Id));
        }
    }
}
