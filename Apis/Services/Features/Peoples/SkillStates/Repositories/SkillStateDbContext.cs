using Microsoft.EntityFrameworkCore;
using Services.Features.Peoples.SkillStates.Models;

namespace Services.Features.Peoples.SkillStates.Repositories
{
    public partial class SkillStateDbContext : DbContext
    {
        public SkillStateDbContext() { }

        public SkillStateDbContext(DbContextOptions<SkillStateDbContext> options)
            : base(options) { }

        public virtual DbSet<SkillState> SkillStates { get; set; }
    }
}
