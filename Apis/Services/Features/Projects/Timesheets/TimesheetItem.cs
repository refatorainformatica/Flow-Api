using System.ComponentModel.DataAnnotations.Schema;
using Services.Features.Projects.ProjectTasks;
using Services.Features.Projects.Timesheets.Events;
using Shared.Domain.Abstractions;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Projects.Timesheets
{
    [Table("TimesheetItems", Schema = "Projects")]
    public partial class TimesheetItem : BaseEntity
    {
        public int TimesheetId { get; set; }

        public int? TaskId { get; set; }

        public bool? Paymentable { get; set; }

        [ForeignKey(nameof(TaskId))]
        [InverseProperty(nameof(Task.TimesheetItems))]
        public virtual ProjectTask Task { get; set; }

        [ForeignKey(nameof(TimesheetId))]
        [InverseProperty(nameof(Timesheet.TimesheetItems))]
        public virtual Timesheet Timesheet { get; set; }

        public override void OnCreatedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new TimesheetCreatedEvent(Id));
        }

        public override void OnEditedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new TimesheetEditedEvent(Id));
        }

        public override void OnRemovedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new TimesheetRemovedEvent(Id));
        }
    }
}
