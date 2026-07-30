using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Services.Features.Peoples.Sellers.Models.Events;
using Shared.Domain.Abstractions;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Peoples.Sellers.Models
{
    [Table("Sellers", Schema = "Peoples")]
    public partial class Seller : BaseEntity
    {
        public Seller()
        {
            Documents = [];
        }

        public Seller(
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

        public Seller(int id, DateTime editedAt, string editedBy, DateTime deletedAt)
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

        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        [StringLength(256)]
        public string Email { get; set; }

        [StringLength(30)]
        public string PhoneNumber { get; set; }

        public string Picture { get; set; }

        [InverseProperty(nameof(SellerDocument.Seller))]
        public virtual ICollection<SellerDocument> Documents { get; set; }

        public override void OnCreatedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new SellerCreatedEvent(Id));
        }

        public override void OnEditedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new SellerEditedEvent(Id));
        }

        public override void OnRemovedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new SellerRemovedEvent(Id));
        }
    }
}
