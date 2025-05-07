using CleanArchitectureSystem.Domain;
using CleanArchitectureSystem.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitectureSystem.Persistence.DatabaseContext
{
    public class CleanArchDbContext(DbContextOptions<CleanArchDbContext> options) : DbContext(options)
    {
        //public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<BatchSerial> BatchSerials { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<MainSerial> MainSerials { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CleanArchDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in base.ChangeTracker.Entries<BaseEntity>()
                .Where(q => q.State == EntityState.Added || q.State == EntityState.Modified))
            {
                entry.Entity.ModifiedDate = DateTime.UtcNow;

                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedDate = DateTime.UtcNow;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
