using Microsoft.EntityFrameworkCore;
using Services.Features.Financials.InvoiceStates.Models;

namespace Services.Features.Financials.InvoiceStates.Repositories
{
    public partial class InvoiceStateDbContext : DbContext
    {
        public InvoiceStateDbContext() { }

        public InvoiceStateDbContext(DbContextOptions<InvoiceStateDbContext> options)
            : base(options) { }

        public virtual DbSet<InvoiceState> InvoiceStates { get; set; }
    }
}
