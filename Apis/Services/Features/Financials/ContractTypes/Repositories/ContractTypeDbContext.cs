using Microsoft.EntityFrameworkCore;
using Services.Features.Financials.ContractTypes.Models;

namespace Services.Features.Financials.ContractTypes.Repositories
{
    public partial class ContractTypeDbContext : DbContext
    {
        public ContractTypeDbContext() { }

        public ContractTypeDbContext(DbContextOptions<ContractTypeDbContext> options)
            : base(options) { }

        public virtual DbSet<ContractType> ContractTypes { get; set; }
    }
}
