using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Services.Features.Sales.Opportunities;
using Services.Features.Sales.OpportunityStates.Events;
using Shared.Domain.Abstractions;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Sales.OpportunityStates
{
    [Table("OpportunityStates", Schema = "Sales")]
    public partial class OpportunityState : BaseEntity
    {
        public OpportunityState()
        {
            Opportunities = [];
        }

        [Required]
        [StringLength(256)]
        public string Name { get; set; }

        [InverseProperty(nameof(Opportunity.OpportunityState))]
        public virtual ICollection<Opportunity> Opportunities { get; set; }

        public override void OnCreatedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new OpportunityStateCreatedEvent(Id));
        }

        public override void OnEditedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new OpportunityStateEditedEvent(Id));
        }

        public override void OnRemovedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new OpportunityStateRemovedEvent(Id));
        }
    }
}
