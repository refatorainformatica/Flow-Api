using Microsoft.EntityFrameworkCore;
using Services.Features.Peoples.SkillCategories.Models;

namespace Services.Features.Peoples.SkillCategories.Repositories
{
    public partial class SkillCategoryDbContext : DbContext
    {
        public SkillCategoryDbContext() { }

        public SkillCategoryDbContext(DbContextOptions<SkillCategoryDbContext> options)
            : base(options) { }

        public virtual DbSet<SkillCategory> SkillCategories { get; set; }
    }
}
