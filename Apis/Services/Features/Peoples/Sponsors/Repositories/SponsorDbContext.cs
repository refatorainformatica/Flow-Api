using Microsoft.EntityFrameworkCore;
using Services.Features.Financials.CurrencyTypes.Models;
using Services.Features.Peoples.Sponsors.Models;

namespace Services.Features.Peoples.Sponsors.Repositories
{
    public partial class SponsorDbContext : DbContext
    {
        public SponsorDbContext() { }

        public SponsorDbContext(DbContextOptions<SponsorDbContext> options)
            : base(options) { }

        public virtual DbSet<Sponsor> Sponsors { get; set; }
        public virtual DbSet<SponsorDocument> SponsorDocuments { get; set; }
        public virtual DbSet<CurrencyType> CurrencyTypes { get; set; }
    }
}
