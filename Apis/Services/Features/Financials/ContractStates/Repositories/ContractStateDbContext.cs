using Microsoft.EntityFrameworkCore;
using Services.Features.Financials.ContractStates.Models;

namespace Services.Features.Financials.ContractStates.Repositories
{
    public partial class ContractStateDbContext : DbContext
    {
        public ContractStateDbContext() { }

        public ContractStateDbContext(DbContextOptions<ContractStateDbContext> options)
            : base(options) { }

        public virtual DbSet<ContractState> ContractStates { get; set; }
    }
}
