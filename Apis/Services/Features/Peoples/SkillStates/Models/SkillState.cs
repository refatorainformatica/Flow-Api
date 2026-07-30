using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Services.Features.Peoples.Skills.Models;
using Services.Features.Peoples.SkillStates.Models.Events;
using Shared.Domain.Abstractions;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Peoples.SkillStates.Models
{
    [Table("SkillStates", Schema = "Peoples")]
    public partial class SkillState : BaseEntity
    {
        public SkillState()
        {
            Skills = [];
        }

        public SkillState(
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

        public SkillState(int id, DateTime editedAt, string editedBy, DateTime deletedAt)
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

        [InverseProperty(nameof(Skill.SkillState))]
        public virtual ICollection<Skill> Skills { get; set; }

        public override void OnCreatedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new SkillStateCreatedEvent(Id));
        }

        public override void OnEditedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new SkillStateEditedEvent(Id));
        }

        public override void OnRemovedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new SkillStateRemovedEvent(Id));
        }
    }
}
