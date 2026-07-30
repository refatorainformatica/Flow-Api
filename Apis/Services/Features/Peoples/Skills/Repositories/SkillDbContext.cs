using Microsoft.EntityFrameworkCore;
using Services.Features.Peoples.SkillCategories.Models;
using Services.Features.Peoples.SkillLevels.Models;
using Services.Features.Peoples.Skills.Models;
using Services.Features.Peoples.SkillStates.Models;
using Services.Features.Peoples.SkillTypes.Models;
using Services.Features.Peoples.Talents.Models;

namespace Services.Features.Peoples.Skills.Repositories
{
    public partial class SkillDbContext : DbContext
    {
        public SkillDbContext() { }

        public SkillDbContext(DbContextOptions<SkillDbContext> options)
            : base(options) { }

        public virtual DbSet<Skill> Skills { get; set; }
        public virtual DbSet<SkillCategory> SkillCategories { get; set; }
        public virtual DbSet<SkillLevel> SkillLevels { get; set; }
        public virtual DbSet<SkillState> SkillStates { get; set; }
        public virtual DbSet<SkillType> SkillTypes { get; set; }
        public virtual DbSet<Talent> Talents { get; set; }
    }
}
