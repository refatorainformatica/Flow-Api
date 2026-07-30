using Microsoft.EntityFrameworkCore;
using Services.Features.Peoples.Careers.Models;

namespace Services.Features.Peoples.Careers.Repositories
{
    public partial class CareerDbContext : DbContext
    {
        public CareerDbContext() { }

        public CareerDbContext(DbContextOptions<CareerDbContext> options)
            : base(options) { }

        public virtual DbSet<Career> Careers { get; set; }
    }
}
