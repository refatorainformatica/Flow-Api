using Microsoft.EntityFrameworkCore;
using Services.Features.Peoples.SkillTypes.Models;

namespace Services.Features.Peoples.SkillTypes.Repositories
{
    public partial class SkillTypeDbContext : DbContext
    {
        public SkillTypeDbContext() { }

        public SkillTypeDbContext(DbContextOptions<SkillTypeDbContext> options)
            : base(options) { }

        public virtual DbSet<SkillType> SkillTypes { get; set; }
    }
}
