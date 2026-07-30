using Microsoft.EntityFrameworkCore;
using Services.Features.Peoples.ActivityBranchs.Models;

namespace Services.Features.Peoples.ActivityBranchs.Repositories
{
    public partial class ActivityBranchDbContext : DbContext
    {
        public ActivityBranchDbContext() { }

        public ActivityBranchDbContext(DbContextOptions<ActivityBranchDbContext> options)
            : base(options) { }

        public virtual DbSet<ActivityBranch> ActivityBranchs { get; set; }
    }
}
