using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Services.Features.Projects.Timesheets;
using Services.Features.Projects.TimesheetStates.Events;
using Shared.Domain.Abstractions;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Projects.TimesheetStates
{
    [Table("TimesheetStates", Schema = "Projects")]
    public partial class TimesheetState : BaseEntity
    {
        public TimesheetState()
        {
            Timesheets = [];
        }

        [Required]
        [StringLength(256)]
        public string Description { get; set; }

        public string Picture { get; set; }

        [InverseProperty(nameof(Timesheet.TimesheetState))]
        public virtual ICollection<Timesheet> Timesheets { get; set; }

        public override void OnCreatedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new TimesheetStateCreatedEvent(Id));
        }

        public override void OnEditedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new TimesheetStateEditedEvent(Id));
        }

        public override void OnRemovedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new TimesheetStateRemovedEvent(Id));
        }
    }
}
