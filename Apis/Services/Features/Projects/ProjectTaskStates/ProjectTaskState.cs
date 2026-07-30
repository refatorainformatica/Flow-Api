using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Services.Features.Projects.ProjectTasks;
using Services.Features.Projects.ProjectTaskStates.Events;
using Shared.Domain.Abstractions;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Projects.ProjectTaskStates
{
    [Table("ProjectTaskStates", Schema = "Projects")]
    public partial class ProjectTaskState : BaseEntity
    {
        public ProjectTaskState()
        {
            ProjectTasks = [];
        }

        [Required]
        [StringLength(256)]
        public string Description { get; set; }

        public string Picture { get; set; }

        [InverseProperty(nameof(ProjectTask.ProjectTaskState))]
        public virtual ICollection<ProjectTask> ProjectTasks { get; set; }

        public override void OnCreatedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new ProjectTaskStateCreatedEvent(Id));
        }

        public override void OnEditedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new ProjectTaskStateEditedEvent(Id));
        }

        public override void OnRemovedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new ProjectTaskStateRemovedEvent(Id));
        }
    }
}
