using Microsoft.EntityFrameworkCore;
using Services.Features.Financials.CostCenters.Models;

namespace Services.Features.Financials.CostCenters.Repositories
{
    public partial class CostCenterDbContext : DbContext
    {
        public CostCenterDbContext() { }

        public CostCenterDbContext(DbContextOptions<CostCenterDbContext> options)
            : base(options) { }

        public virtual DbSet<CostCenter> CostCenters { get; set; }
    }
}
