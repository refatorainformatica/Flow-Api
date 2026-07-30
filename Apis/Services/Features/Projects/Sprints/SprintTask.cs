using System.ComponentModel.DataAnnotations.Schema;
using Services.Features.Projects.ProjectTasks;
using Services.Features.Projects.Sprints.Events;
using Shared.Domain.Abstractions;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Projects.Sprints
{
    [Table("SprintTasks", Schema = "Projects")]
    public partial class SprintTask : BaseEntity
    {
        public int SprintId { get; set; }

        public int ProjectTaskId { get; set; }

        [ForeignKey(nameof(SprintId))]
        [InverseProperty(nameof(ProjectSprint.SprintTasks))]
        public virtual Sprint ProjectSprint { get; set; }

        [ForeignKey(nameof(ProjectTaskId))]
        [InverseProperty(nameof(SprintTask))]
        public virtual ProjectTask ProjectTask { get; set; }

        public override void OnCreatedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new SprintCreatedEvent(Id));
        }

        public override void OnEditedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new SprintEditedEvent(Id));
        }

        public override void OnRemovedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new SprintRemovedEvent(Id));
        }
    }
}
