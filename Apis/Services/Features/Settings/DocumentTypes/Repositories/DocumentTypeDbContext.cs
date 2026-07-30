using Microsoft.EntityFrameworkCore;
using Services.Features.Settings.DocumentTypes.Models;

namespace Services.Features.Settings.DocumentTypes.Repositories
{
    public partial class DocumentTypeDbContext : DbContext
    {
        public DocumentTypeDbContext() { }

        public DocumentTypeDbContext(DbContextOptions<DocumentTypeDbContext> options)
            : base(options) { }

        public virtual DbSet<DocumentType> DocumentTypes { get; set; }
    }
}
