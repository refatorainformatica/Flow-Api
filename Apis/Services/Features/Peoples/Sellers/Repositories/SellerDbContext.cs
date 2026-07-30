using Microsoft.EntityFrameworkCore;
using Services.Features.Peoples.Sellers.Models;

namespace Services.Features.Peoples.Sellers.Repositories
{
    public partial class SellerDbContext : DbContext
    {
        public SellerDbContext() { }

        public SellerDbContext(DbContextOptions<SellerDbContext> options)
            : base(options) { }

        public virtual DbSet<Seller> Sellers { get; set; }
        public virtual DbSet<SellerDocument> SellerDocuments { get; set; }
    }
}
