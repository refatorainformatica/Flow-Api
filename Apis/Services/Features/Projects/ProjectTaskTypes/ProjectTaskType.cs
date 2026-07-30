using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Services.Features.Projects.ProjectTasks;
using Services.Features.Projects.ProjectTaskTypes.Events;
using Shared.Domain.Abstractions;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Projects.ProjectTaskTypes
{
    [Table("ProjectTaskTypes", Schema = "Projects")]
    public partial class ProjectTaskType : BaseEntity
    {
        public ProjectTaskType()
        {
            ProjectTasks = [];
        }

        [Required]
        [StringLength(256)]
        public string Description { get; set; }

        public string Picture { get; set; }

        [InverseProperty(nameof(ProjectTask.ProjectTaskType))]
        public virtual ICollection<ProjectTask> ProjectTasks { get; set; }

        public override void OnCreatedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new ProjectTaskTypeCreatedEvent(Id));
        }

        public override void OnEditedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new ProjectTaskTypeEditedEvent(Id));
        }

        public override void OnRemovedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new ProjectTaskTypeRemovedEvent(Id));
        }
    }
}
