using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Services.Features.Peoples.SkillLevels.Models.Events;
using Services.Features.Peoples.Skills.Models;
using Shared.Domain.Abstractions;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Peoples.SkillLevels.Models
{
    [Table("SkillLevels", Schema = "Peoples")]
    public partial class SkillLevel : BaseEntity
    {
        public SkillLevel()
        {
            SkillLevelMaxes = [];
            Skills = [];
        }

        public SkillLevel(
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
            SkillLevelMaxes = [];
            Skills = [];
        }

        public SkillLevel(int id, DateTime editedAt, string editedBy, DateTime deletedAt)
        {
            Id = id;
            EditedAt = editedAt;
            EditedBy = editedBy;
            DeletedAt = deletedAt;
            SkillLevelMaxes = [];
            Skills = [];
        }

        [Required]
        [StringLength(256)]
        public string Description { get; set; }

        public string Picture { get; set; }

        [InverseProperty(nameof(Skill.SkillLevelMax))]
        public virtual ICollection<Skill> SkillLevelMaxes { get; set; }

        [InverseProperty(nameof(Skill.SkillLevel))]
        public virtual ICollection<Skill> Skills { get; set; }

        public override void OnCreatedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new SkillLevelCreatedEvent(Id));
        }

        public override void OnEditedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new SkillLevelEditedEvent(Id));
        }

        public override void OnRemovedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new SkillLevelRemovedEvent(Id));
        }
    }
}
