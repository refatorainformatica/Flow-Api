using Microsoft.EntityFrameworkCore;
using Services.Features.Peoples.MaritalStates.Models;

namespace Services.Features.Peoples.MaritalStates.Repositories
{
    public partial class MaritalStateDbContext : DbContext
    {
        public MaritalStateDbContext() { }

        public MaritalStateDbContext(DbContextOptions<MaritalStateDbContext> options)
            : base(options) { }

        public virtual DbSet<MaritalState> MaritalStates { get; set; }
    }
}
