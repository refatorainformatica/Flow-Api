//using Flow.Domain.Core;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;
//using Shared.Domain.Abstractions;

//namespace Shared.Infrastructure.Persistences
//{
//    public partial class ApplicationIdentityDbContext
//        : IdentityDbContext<ApplicationUser, ApplicationRole, string>
//    {
//        public ApplicationIdentityDbContext(DbContextOptions<ApplicationIdentityDbContext> options)
//            : base(options) { }

//        public ApplicationIdentityDbContext() { }

//        private partial void OnModelBuilding(ModelBuilder builder);

//        protected override void OnModelCreating(ModelBuilder builder)
//        {
//            base.OnModelCreating(builder);

//            builder.HasDefaultSchema("Users");

//            builder.Entity<ApplicationUser>(entity => entity.ToTable("Users"));
//            builder.Entity<ApplicationRole>(entity => entity.ToTable("Roles"));
//            builder.Entity<IdentityUserRole<string>>(entity => entity.ToTable("UserRoles"));
//            builder.Entity<IdentityUserClaim<string>>(entity => entity.ToTable("UserClaims"));
//            builder.Entity<IdentityUserLogin<string>>(entity => entity.ToTable("UserLogins"));
//            builder.Entity<IdentityRoleClaim<string>>(entity => entity.ToTable("RoleClaims"));

//            builder
//                .Entity<ApplicationUser>()
//                .HasMany(u => u.Roles)
//                .WithMany(r => r.Users)
//                .UsingEntity<IdentityUserRole<string>>();

//            OnModelBuilding(builder);
//        }
//    }
//}
