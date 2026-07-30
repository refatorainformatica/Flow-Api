using Microsoft.EntityFrameworkCore;
using Services.Features.Financials.ExpenseTypes.Models;

namespace Services.Features.Financials.ExpenseTypes.Repositories
{
    public partial class ExpenseTypeDbContext : DbContext
    {
        public ExpenseTypeDbContext() { }

        public ExpenseTypeDbContext(DbContextOptions<ExpenseTypeDbContext> options)
            : base(options) { }

        public virtual DbSet<ExpenseType> ExpenseTypes { get; set; }
    }
}
