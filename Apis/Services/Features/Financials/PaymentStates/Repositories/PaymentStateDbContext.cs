using Microsoft.EntityFrameworkCore;
using Services.Features.Financials.PaymentStates.Models;

namespace Services.Features.Financials.PaymentStates.Repositories
{
    public partial class PaymentStateDbContext : DbContext
    {
        public PaymentStateDbContext() { }

        public PaymentStateDbContext(DbContextOptions<PaymentStateDbContext> options)
            : base(options) { }

        public virtual DbSet<PaymentState> PaymentStates { get; set; }
    }
}
