using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Services.Features.Sales.Contacts.Events;
using Services.Features.Sales.Opportunities;
using Shared.Domain.Abstractions;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Sales.Contacts
{
    [Table("Contacts", Schema = "Sales")]
    public partial class Contact : BaseEntity
    {
        [Required]
        [StringLength(256)]
        public string Email { get; set; }

        [StringLength(256)]
        public string Company { get; set; }

        [StringLength(256)]
        public string LastName { get; set; }

        [StringLength(256)]
        public string FirstName { get; set; }

        [StringLength(256)]
        public string Phone { get; set; }

        [InverseProperty(nameof(Opportunity.Contact))]
        public virtual ICollection<Opportunity> Opportunities { get; set; }

        public override void OnCreatedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new ContactCreatedEvent(Id));
        }

        public override void OnEditedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new ContactEditedEvent(Id));
        }

        public override void OnRemovedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new ContactRemovedEvent(Id));
        }
    }
}
