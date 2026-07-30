using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Services.Features.Peoples.SkillCategories.Models;
using Services.Features.Peoples.SkillLevels.Models;
using Services.Features.Peoples.Skills.Models.Events;
using Services.Features.Peoples.SkillStates.Models;
using Services.Features.Peoples.SkillTypes.Models;
using Services.Features.Peoples.Talents.Models;
using Shared.Domain.Abstractions;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Peoples.Skills.Models
{
    [Table("Skills", Schema = "Peoples")]
    public partial class Skill : BaseEntity
    {
        public Skill() { }

        public Skill(
            int id,
            int talentId,
            string description,
            string institute,
            int skillTypeId,
            int skillCategoryId,
            int skillLevelId,
            int skillLevelMaxId,
            int skillStateId,
            DateTime startDate,
            DateTime? endDate,
            string picture,
            DateTime createdAt,
            string createdBy,
            DateTime editedAt,
            string editedBy,
            DateTime? deletedAt = null
        )
        {
            Id = id;
            TalentId = talentId;
            Description = description;
            Institute = institute;
            SkillTypeId = skillTypeId;
            SkillCategoryId = skillCategoryId;
            SkillLevelId = skillLevelId;
            SkillLevelMaxId = skillLevelMaxId;
            SkillStateId = skillStateId;
            StartDate = startDate;
            EndDate = endDate;
            Picture = picture;
            CreatedAt = createdAt;
            CreatedBy = createdBy;
            EditedAt = editedAt;
            EditedBy = editedBy;
            DeletedAt = deletedAt;
        }

        public Skill(int id, DateTime editedAt, string editedBy, DateTime deletedAt)
        {
            Id = id;
            EditedAt = editedAt;
            EditedBy = editedBy;
            DeletedAt = deletedAt;
        }

        public int TalentId { get; set; }

        [Required]
        [StringLength(256)]
        public string Description { get; set; }

        [Required]
        [StringLength(256)]
        public string Institute { get; set; }

        public int SkillTypeId { get; set; }

        public int SkillCategoryId { get; set; }

        public int SkillLevelId { get; set; }

        public int SkillLevelMaxId { get; set; }

        public int SkillStateId { get; set; }

        [Column(TypeName = "date")]
        public DateTime StartDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? EndDate { get; set; }

        public string Picture { get; set; }

        [ForeignKey(nameof(SkillCategoryId))]
        [InverseProperty(nameof(SkillCategory.Skills))]
        public virtual SkillCategory SkillCategory { get; set; }

        [ForeignKey(nameof(SkillLevelId))]
        [InverseProperty(nameof(SkillLevel.Skills))]
        public virtual SkillLevel SkillLevel { get; set; }

        [ForeignKey(nameof(SkillLevelMaxId))]
        [InverseProperty(nameof(SkillLevel.SkillLevelMaxes))]
        public virtual SkillLevel SkillLevelMax { get; set; }

        [ForeignKey(nameof(SkillStateId))]
        [InverseProperty(nameof(SkillState.Skills))]
        public virtual SkillState SkillState { get; set; }

        [ForeignKey(nameof(SkillTypeId))]
        [InverseProperty(nameof(SkillType.Skills))]
        public virtual SkillType SkillType { get; set; }

        [ForeignKey(nameof(TalentId))]
        [InverseProperty(nameof(Talent.Skills))]
        public virtual Talent Talent { get; set; }

        public override void OnCreatedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new SkillCreatedEvent(Id));
        }

        public override void OnEditedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new SkillEditedEvent(Id));
        }

        public override void OnRemovedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new SkillRemovedEvent(Id));
        }
    }
}
