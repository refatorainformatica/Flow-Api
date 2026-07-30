using Microsoft.EntityFrameworkCore;
using Services.Features.Financials.Invoices.Models;
using Services.Features.Financials.InvoiceStates.Models;
using Services.Features.Financials.InvoiceTypes.Models;

namespace Services.Features.Financials.Invoices.Repositories
{
    public partial class InvoiceDbContext : DbContext
    {
        public InvoiceDbContext() { }

        public InvoiceDbContext(DbContextOptions<InvoiceDbContext> options)
            : base(options) { }

        public virtual DbSet<Invoice> Invoices { get; set; }
        public virtual DbSet<InvoiceItem> InvoiceItems { get; set; }
        public virtual DbSet<InvoiceState> InvoiceStates { get; set; }
        public virtual DbSet<InvoiceType> InvoiceTypes { get; set; }
    }
}
