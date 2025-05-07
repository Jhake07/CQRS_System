using CleanArchitectureSystem.Identity.EntityModel;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitectureSystem.Identity.DatabaseContext
{
    public class CleanArchIdentityDbContext(DbContextOptions<CleanArchIdentityDbContext> options) : IdentityDbContext<AppUser>(options)
    {
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(CleanArchIdentityDbContext).Assembly);
        }
    }
}
