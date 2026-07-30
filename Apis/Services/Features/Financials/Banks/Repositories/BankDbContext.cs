using Microsoft.EntityFrameworkCore;
using Services.Features.Financials.Banks.Models;

namespace Services.Features.Financials.Banks.Repositories
{
    public partial class BankDbContext : DbContext
    {
        public BankDbContext() { }

        public BankDbContext(DbContextOptions<BankDbContext> options)
            : base(options) { }

        public virtual DbSet<Bank> Banks { get; set; }
    }
}
