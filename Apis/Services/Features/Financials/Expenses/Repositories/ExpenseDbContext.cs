using Microsoft.EntityFrameworkCore;
using Services.Features.Financials.Expenses.Models;
using Services.Features.Financials.ExpenseTypes.Models;

namespace Services.Features.Financials.Expenses.Repositories
{
    public partial class ExpenseDbContext : DbContext
    {
        public ExpenseDbContext() { }

        public ExpenseDbContext(DbContextOptions<ExpenseDbContext> options)
            : base(options) { }

        public virtual DbSet<Expense> Expenses { get; set; }
        public virtual DbSet<ExpenseType> ExpenseTypes { get; set; }
    }
}
