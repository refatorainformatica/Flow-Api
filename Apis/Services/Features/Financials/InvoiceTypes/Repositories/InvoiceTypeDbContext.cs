using Microsoft.EntityFrameworkCore;
using Services.Features.Financials.InvoiceTypes.Models;

namespace Services.Features.Financials.InvoiceTypes.Repositories
{
    public partial class InvoiceTypeDbContext : DbContext
    {
        public InvoiceTypeDbContext() { }

        public InvoiceTypeDbContext(DbContextOptions<InvoiceTypeDbContext> options)
            : base(options) { }

        public virtual DbSet<InvoiceType> InvoiceTypes { get; set; }
    }
}
