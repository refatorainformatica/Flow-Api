using Microsoft.EntityFrameworkCore;
using Services.Features.Financials.CurrencyTypes.Models;

namespace Services.Features.Financials.CurrencyTypes.Repositories
{
    public partial class CurrencyTypeDbContext : DbContext
    {
        public CurrencyTypeDbContext() { }

        public CurrencyTypeDbContext(DbContextOptions<CurrencyTypeDbContext> options)
            : base(options) { }

        public virtual DbSet<CurrencyType> CurrencyTypes { get; set; }
    }
}
