using Microsoft.EntityFrameworkCore;
using Services.Features.Financials.Contracts.Models;
using Services.Features.Financials.CurrencyTypes.Models;
using Services.Features.Financials.Invoices.Models;
using Services.Features.Peoples.ActivityBranchs.Models;
using Services.Features.Peoples.JuridicalNatures.Models;
using Services.Features.Peoples.Suppliers.Models;
using Services.Features.Peoples.Talents.Models;
using Services.Features.Projects.Allocations;
using Services.Features.Projects.Timesheets;

namespace Services.Features.Peoples.Suppliers.Repositories
{
    public partial class SupplierDbContext : DbContext
    {
        public SupplierDbContext() { }

        public SupplierDbContext(DbContextOptions<SupplierDbContext> options)
            : base(options) { }

        public virtual DbSet<Supplier> Suppliers { get; set; }
        public virtual DbSet<SupplierDocument> SupplierDocuments { get; set; }
        public virtual DbSet<ActivityBranch> ActivityBranchs { get; set; }
        public virtual DbSet<Contract> Contracts { get; set; }
        public virtual DbSet<CurrencyType> CurrencyTypes { get; set; }
        public virtual DbSet<JuridicalNature> JuridicalNatures { get; set; }
        public virtual DbSet<Talent> Talents { get; set; }
        public virtual DbSet<Allocation> Allocations { get; set; }
        public virtual DbSet<Invoice> Invoices { get; set; }
        public virtual DbSet<Timesheet> Timesheets { get; set; }
    }
}
