using Microsoft.EntityFrameworkCore;
using Services.Features.Peoples.Customers.Models;
using Services.Features.Projects.Projects.Models;

namespace Services.Features.Peoples.Customers.Repositories
{
    public partial class CustomerDbContext : DbContext
    {
        public CustomerDbContext() { }

        public CustomerDbContext(DbContextOptions<CustomerDbContext> options)
            : base(options) { }

        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<CustomerDocument> CustomerDocuments { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
    }
}
