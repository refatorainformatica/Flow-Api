using Microsoft.EntityFrameworkCore;
using Services.Features.Peoples.JuridicalNatures.Models;

namespace Services.Features.Peoples.JuridicalNatures.Repositories
{
    public partial class JuridicalNatureDbContext : DbContext
    {
        public JuridicalNatureDbContext() { }

        public JuridicalNatureDbContext(DbContextOptions<JuridicalNatureDbContext> options)
            : base(options) { }

        public virtual DbSet<JuridicalNature> JuridicalNatures { get; set; }
    }
}
