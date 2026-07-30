using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Services.Features.Projects.PriorityTypes;
using Services.Features.Projects.Projects.Models;
using Services.Features.Projects.ProjectTasks.Events;
using Services.Features.Projects.ProjectTaskStates;
using Services.Features.Projects.ProjectTaskTypes;
using Services.Features.Projects.Sprints;
using Services.Features.Projects.Timesheets;
using Shared.Domain.Abstractions;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Projects.ProjectTasks
{
    [Table("ProjectTasks", Schema = "Projects")]
    public partial class ProjectTask : BaseEntity
    {
        public ProjectTask()
        {
            TimesheetItems = [];
        }

        public int? ProjectId { get; set; }

        public int? ProjectTaskTypeId { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal? FunctionPointsValue { get; set; }

        public int? PriorityTypeId { get; set; }

        [Required]
        public string Description { get; set; }

        [Column(TypeName = "date")]
        public DateTime? StartDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? EndDate { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal? EstimatedTimeInSeconds { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal? TimeSpentInSeconds { get; set; }

        public bool? Recurrent { get; set; }

        public bool? Paymentable { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal? PaymentableTimeInSeconds { get; set; }

        public int? ProjectTaskId { get; set; }

        public int ProjectTaskStateId { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal? CompletionPercentage { get; set; }

        public string Picture { get; set; }

        [ForeignKey(nameof(PriorityTypeId))]
        [InverseProperty(nameof(PriorityType.ProjectTasks))]
        public virtual PriorityType PriorityType { get; set; }

        [ForeignKey(nameof(ProjectId))]
        [InverseProperty(nameof(Project.ProjectTasks))]
        public virtual Project Project { get; set; }

        [ForeignKey(nameof(ProjectTaskTypeId))]
        [InverseProperty(nameof(ProjectTaskType.ProjectTasks))]
        public virtual ProjectTaskType ProjectTaskType { get; set; }

        [ForeignKey(nameof(ProjectTaskStateId))]
        [InverseProperty(nameof(ProjectTaskState.ProjectTasks))]
        public virtual ProjectTaskState ProjectTaskState { get; set; }

        [InverseProperty(nameof(SprintTask.ProjectTask))]
        public virtual SprintTask SprintTask { get; set; }

        [InverseProperty(nameof(TimesheetItem.Task))]
        public virtual ICollection<TimesheetItem> TimesheetItems { get; set; }

        public override void OnCreatedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new ProjectTaskCreatedEvent(Id));
        }

        public override void OnEditedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new ProjectTaskEditedEvent(Id));
        }

        public override void OnRemovedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new ProjectTaskRemovedEvent(Id));
        }
    }
}
