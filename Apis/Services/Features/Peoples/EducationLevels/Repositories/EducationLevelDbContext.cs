using Microsoft.EntityFrameworkCore;
using Services.Features.Peoples.EducationLevels.Models;

namespace Services.Features.Peoples.EducationLevels.Repositories
{
    public partial class EducationLevelDbContext : DbContext
    {
        public EducationLevelDbContext() { }

        public EducationLevelDbContext(DbContextOptions<EducationLevelDbContext> options)
            : base(options) { }

        public virtual DbSet<EducationLevel> EducationLevels { get; set; }
    }
}
