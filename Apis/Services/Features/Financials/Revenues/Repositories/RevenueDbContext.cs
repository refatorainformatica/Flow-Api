using Microsoft.EntityFrameworkCore;
using Services.Features.Financials.CashFlows.Models;
using Services.Features.Financials.CostCenters.Models;
using Services.Features.Financials.Revenues.Models;
using Services.Features.Financials.RevenueTypes.Models;

namespace Services.Features.Financials.Revenues.Repositories
{
    public partial class RevenueDbContext : DbContext
    {
        public RevenueDbContext() { }

        public RevenueDbContext(DbContextOptions<RevenueDbContext> options)
            : base(options) { }

        public virtual DbSet<Revenue> Revenues { get; set; }
        public virtual DbSet<RevenueType> RevenueTypes { get; set; }
        public virtual DbSet<RevenueType> PaymentStates { get; set; }
        public virtual DbSet<CostCenter> CostCenters { get; set; }
        public virtual DbSet<CashFlow> CashFlows { get; set; }
    }
}
