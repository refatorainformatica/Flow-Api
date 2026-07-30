using Microsoft.EntityFrameworkCore;
using Services.Features.Peoples.ProfessionalProfiles.Models;

namespace Services.Features.Peoples.ProfessionalProfiles.Repositories
{
    public partial class ProfessionalProfileDbContext : DbContext
    {
        public ProfessionalProfileDbContext() { }

        public ProfessionalProfileDbContext(DbContextOptions<ProfessionalProfileDbContext> options)
            : base(options) { }

        public virtual DbSet<ProfessionalProfile> ProfessionalProfiles { get; set; }
    }
}
