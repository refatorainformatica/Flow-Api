using Microsoft.EntityFrameworkCore;
using Services.Features.Financials.Contracts.Models;
using Services.Features.Financials.ContractStates.Models;
using Services.Features.Financials.ContractTypes.Models;

namespace Services.Features.Financials.Contracts.Repositories
{
    public partial class ContractDbContext : DbContext
    {
        public ContractDbContext() { }

        public ContractDbContext(DbContextOptions<ContractDbContext> options)
            : base(options) { }

        public virtual DbSet<Contract> Contracts { get; set; }
        public virtual DbSet<ContractDocument> ContractDocuments { get; set; }
        public virtual DbSet<ContractState> ContractStates { get; set; }
        public virtual DbSet<ContractSubscription> ContractSubscriptions { get; set; }
        public virtual DbSet<ContractType> ContractTypes { get; set; }
    }
}
