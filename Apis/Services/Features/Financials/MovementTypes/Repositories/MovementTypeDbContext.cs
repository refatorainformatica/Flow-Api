using Microsoft.EntityFrameworkCore;
using Services.Features.Financials.MovementTypes.Models;

namespace Services.Features.Financials.MovementTypes.Repositories
{
    public partial class MovementTypeDbContext : DbContext
    {
        public MovementTypeDbContext() { }

        public MovementTypeDbContext(DbContextOptions<MovementTypeDbContext> options)
            : base(options) { }

        public virtual DbSet<MovementType> MovementTypes { get; set; }
    }
}
