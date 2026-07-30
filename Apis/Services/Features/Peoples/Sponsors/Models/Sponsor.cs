using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Services.Features.Financials.CurrencyTypes.Models;
using Services.Features.Peoples.Sponsors.Models.Events;
using Shared.Domain.Abstractions;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Peoples.Sponsors.Models
{
    [Table("Sponsors", Schema = "Peoples")]
    public partial class Sponsor : BaseEntity
    {
        public Sponsor()
        {
            Documents = [];
        }

        public Sponsor(
            int id,
            string name,
            string addressLine1,
            string addressLine2,
            string email,
            string phoneNumber,
            string picture,
            DateTime createdAt,
            string createdBy,
            DateTime editedAt,
            string editedBy
        )
        {
            Id = id;
            Name = name;
            AddressLine1 = addressLine1;
            AddressLine2 = addressLine2;
            Email = email;
            PhoneNumber = phoneNumber;
            Picture = picture;
            CreatedAt = createdAt;
            CreatedBy = createdBy;
            EditedAt = editedAt;
            EditedBy = editedBy;
            Documents = [];
        }

        public Sponsor(int id, DateTime editedAt, string editedBy, DateTime deletedAt)
        {
            Id = id;
            EditedAt = editedAt;
            EditedBy = editedBy;
            DeletedAt = deletedAt;
            Documents = [];
        }

        [Required]
        [StringLength(256)]
        public string Name { get; set; }

        public int? PaymentingCurrencyTypeId { get; set; }

        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        [StringLength(256)]
        public string Email { get; set; }

        [StringLength(30)]
        public string PhoneNumber { get; set; }

        public string Picture { get; set; }

        [ForeignKey(nameof(PaymentingCurrencyTypeId))]
        [InverseProperty(nameof(CurrencyType.Sponsors))]
        public virtual CurrencyType PaymentingCurrencyType { get; set; }

        [InverseProperty(nameof(SponsorDocument.Sponsor))]
        public virtual ICollection<SponsorDocument> Documents { get; set; }

        public override void OnCreatedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new SponsorCreatedEvent(Id));
        }

        public override void OnEditedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new SponsorEditedEvent(Id));
        }

        public override void OnRemovedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new SponsorRemovedEvent(Id));
        }
    }
}
