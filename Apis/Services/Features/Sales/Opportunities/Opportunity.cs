using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Services.Features.Sales.Contacts;
using Services.Features.Sales.Opportunities.Events;
using Services.Features.Sales.SaleTasks;
using Shared.Domain.Abstractions;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Sales.Opportunities
{
    [Table("Opportunities", Schema = "Sales")]
    public partial class Opportunity : BaseEntity
    {
        public Opportunity()
        {
            Tasks = [];
        }

        [Column(TypeName = "money")]
        public decimal Amount { get; set; }

        [Required]
        [StringLength(256)]
        public string UserId { get; set; }

        public int ContactId { get; set; }

        public int OpportunityStateId { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime CloseDate { get; set; }

        [Required]
        [StringLength(256)]
        public string Name { get; set; }

        [ForeignKey(nameof(ContactId))]
        [InverseProperty(nameof(Contact.Opportunities))]
        public virtual Contact Contact { get; set; }

        [ForeignKey(nameof(OpportunityStateId))]
        [InverseProperty(nameof(OpportunityState.Opportunities))]
        public virtual OpportunityStates.OpportunityState OpportunityState { get; set; }

        [InverseProperty(nameof(SaleTask.Opportunity))]
        public virtual ICollection<SaleTask> Tasks { get; set; }

        public override void OnCreatedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new OpportunityCreatedEvent(Id));
        }

        public override void OnEditedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new OpportunityEditedEvent(Id));
        }

        public override void OnRemovedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new OpportunityRemovedEvent(Id));
        }
    }
}
