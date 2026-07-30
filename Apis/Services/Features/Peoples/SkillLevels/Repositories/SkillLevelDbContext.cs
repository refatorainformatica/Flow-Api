using Microsoft.EntityFrameworkCore;
using Services.Features.Peoples.SkillLevels.Models;

namespace Services.Features.Peoples.SkillLevels.Repositories
{
    public partial class SkillLevelDbContext : DbContext
    {
        public SkillLevelDbContext() { }

        public SkillLevelDbContext(DbContextOptions<SkillLevelDbContext> options)
            : base(options) { }

        public virtual DbSet<SkillLevel> SkillLevels { get; set; }
    }
}
