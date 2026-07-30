using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Services.Features.Peoples.Skills.Models;
using Services.Features.Peoples.SkillTypes.Models.Events;
using Shared.Domain.Abstractions;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Peoples.SkillTypes.Models
{
    [Table("SkillTypes", Schema = "Peoples")]
    public partial class SkillType : BaseEntity
    {
        public SkillType()
        {
            Skills = [];
        }

        public SkillType(
            int id,
            string description,
            string picture,
            DateTime createdAt,
            string createdBy,
            DateTime editedAt,
            string editedBy,
            DateTime? deletedAt = null
        )
        {
            Id = id;
            Description = description;
            Picture = picture;
            CreatedAt = createdAt;
            CreatedBy = createdBy;
            EditedAt = editedAt;
            EditedBy = editedBy;
            DeletedAt = deletedAt;
        }

        public SkillType(int id, DateTime editedAt, string editedBy, DateTime deletedAt)
        {
            Id = id;
            EditedAt = editedAt;
            EditedBy = editedBy;
            DeletedAt = deletedAt;
        }

        [Required]
        [StringLength(256)]
        public string Description { get; set; }

        public string Picture { get; set; }

        [InverseProperty(nameof(Skill.SkillType))]
        public virtual ICollection<Skill> Skills { get; set; }

        public override void OnCreatedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new SkillTypeCreatedEvent(Id));
        }

        public override void OnEditedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new SkillTypeEditedEvent(Id));
        }

        public override void OnRemovedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new SkillTypeRemovedEvent(Id));
        }
    }
}
