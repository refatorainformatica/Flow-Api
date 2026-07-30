using System.ComponentModel.DataAnnotations.Schema;
using Services.Features.Peoples.Suppliers.Models;
using Services.Features.Projects.Timesheets.Events;
using Services.Features.Projects.TimesheetStates;
using Shared.Domain.Abstractions;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Projects.Timesheets
{
    [Table("Timesheets", Schema = "Projects")]
    public partial class Timesheet : BaseEntity
    {
        public Timesheet()
        {
            TimesheetItems = [];
        }

        public int SupplierId { get; set; }

        public string Description { get; set; }

        public int? YearExercise { get; set; }

        public int? MonthExercise { get; set; }

        public int TimesheetStateId { get; set; }

        public string Picture { get; set; }

        [ForeignKey(nameof(SupplierId))]
        [InverseProperty(nameof(Supplier.Timesheets))]
        public virtual Supplier Supplier { get; set; }

        [ForeignKey(nameof(TimesheetStateId))]
        [InverseProperty(nameof(TimesheetState.Timesheets))]
        public virtual TimesheetState TimesheetState { get; set; }

        [InverseProperty(nameof(TimesheetItem.Timesheet))]
        public virtual ICollection<TimesheetItem> TimesheetItems { get; set; }

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
