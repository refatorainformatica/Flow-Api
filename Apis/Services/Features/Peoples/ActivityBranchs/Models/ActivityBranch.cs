using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Services.Features.Peoples.ActivityBranchs.Models.Events;
using Services.Features.Peoples.Suppliers.Models;
using Shared.Domain.Abstractions;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Peoples.ActivityBranchs.Models
{
    [Table("ActivityBranchs", Schema = "Peoples")]
    public partial class ActivityBranch : BaseEntity
    {
        public ActivityBranch()
        {
            PeopleSuppliers = [];
        }

        public ActivityBranch(
            int id,
            string externalCode,
            string description,
            string picture,
            DateTime createdAt,
            string createdBy,
            DateTime editedAt,
            string editedBy
        )
        {
            Id = id;
            ExternalCode = externalCode;
            Description = description;
            Picture = picture;
            CreatedAt = createdAt;
            CreatedBy = createdBy;
            EditedAt = editedAt;
            EditedBy = editedBy;
        }

        public ActivityBranch(int id, DateTime editedAt, string editedBy, DateTime deletedAt)
        {
            Id = id;
            EditedAt = editedAt;
            EditedBy = editedBy;
            DeletedAt = deletedAt;
        }

        [StringLength(256)]
        public string ExternalCode { get; set; }

        [Required]
        [StringLength(256)]
        public string Description { get; set; }

        public string Picture { get; set; }

        [InverseProperty(nameof(Supplier.ActivityBranch))]
        public virtual ICollection<Supplier> PeopleSuppliers { get; set; }

        public override void OnCreatedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new ActivityBranchCreatedEvent(Id));
        }

        public override void OnEditedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new ActivityBranchEditedEvent(Id));
        }

        public override void OnRemovedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new ActivityBranchRemovedEvent(Id));
        }
    }
}
