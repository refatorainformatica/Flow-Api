using Microsoft.EntityFrameworkCore;
using Services.Features.Financials.RevenueTypes.Models;

namespace Services.Features.Financials.RevenueTypes.Repositories
{
    public partial class RevenueTypeDbContext : DbContext
    {
        public RevenueTypeDbContext() { }

        public RevenueTypeDbContext(DbContextOptions<RevenueTypeDbContext> options)
            : base(options) { }

        public virtual DbSet<RevenueType> RevenueTypes { get; set; }
    }
}
