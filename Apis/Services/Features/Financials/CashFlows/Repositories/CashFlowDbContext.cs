using Microsoft.EntityFrameworkCore;
using Services.Features.Financials.CashFlows.Models;

namespace Services.Features.Financials.CashFlows.Repositories
{
    public partial class CashFlowDbContext : DbContext
    {
        public CashFlowDbContext() { }

        public CashFlowDbContext(DbContextOptions<CashFlowDbContext> options)
            : base(options) { }

        public virtual DbSet<CashFlow> CashFlows { get; set; }
    }
}
