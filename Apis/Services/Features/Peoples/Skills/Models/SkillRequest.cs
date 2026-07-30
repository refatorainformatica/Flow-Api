namespace Services.Features.Peoples.Skills.Models
{
    public class SkillRequest
    {
        public int Id { get; set; }
        public int TalentId { get; set; }
        public string Description { get; set; }
        public string Institute { get; set; }
        public int SkillTypeId { get; set; }
        public int SkillCategoryId { get; set; }
        public int SkillLevelId { get; set; }
        public int SkillLevelMaxId { get; set; }
        public int SkillStateId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Picture { get; set; }
    }
}
