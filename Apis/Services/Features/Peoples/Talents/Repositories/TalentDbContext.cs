using Microsoft.EntityFrameworkCore;
using Services.Features.Peoples.Careers.Models;
using Services.Features.Peoples.EducationLevels.Models;
using Services.Features.Peoples.MaritalStates.Models;
using Services.Features.Peoples.Skills.Models;
using Services.Features.Peoples.Suppliers.Models;
using Services.Features.Peoples.Talents.Models;

namespace Services.Features.Peoples.Talents.Repositories
{
    public partial class TalentDbContext : DbContext
    {
        public TalentDbContext() { }

        public TalentDbContext(DbContextOptions<TalentDbContext> options)
            : base(options) { }

        public virtual DbSet<Talent> Talents { get; set; }
        public virtual DbSet<TalentDocument> TalentDocuments { get; set; }
        public virtual DbSet<Career> Careers { get; set; }
        public virtual DbSet<EducationLevel> EducationLevels { get; set; }
        public virtual DbSet<MaritalState> MaritalStates { get; set; }
        public virtual DbSet<Skill> Skills { get; set; }
        public virtual DbSet<Supplier> Suppliers { get; set; }
    }
}
