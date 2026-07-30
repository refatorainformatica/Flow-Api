using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Services.Features.Peoples.Careers.Models.Events;
using Services.Features.Peoples.Talents.Models;
using Shared.Domain.Abstractions;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Peoples.Careers.Models
{
    [Table("Careers", Schema = "Peoples")]
    public partial class Career : BaseEntity
    {
        public Career()
        {
            PeopleTalents = [];
        }

        public Career(
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

        public Career(int id, DateTime editedAt, string editedBy, DateTime deletedAt)
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

        [InverseProperty(nameof(Talent.Career))]
        public virtual ICollection<Talent> PeopleTalents { get; set; }

        public override void OnCreatedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new CareerCreatedEvent(Id));
        }

        public override void OnEditedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new CareerEditedEvent(Id));
        }

        public override void OnRemovedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new CareerRemovedEvent(Id));
        }
    }
}
