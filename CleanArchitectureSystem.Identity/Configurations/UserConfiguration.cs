using CleanArchitectureSystem.Identity.EntityModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitectureSystem.Identity.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            //var hasher = new PasswordHasher<AppUser>();
            //builder.HasData(
            //     new AppUser
            //     {
            //         Id = "AdminUser",
            //         Email = "admin@localhost.com",
            //         NormalizedEmail = "ADMIN@LOCALHOST.COM",
            //         FirstName = "System",
            //         LastName = "Admin",
            //         UserName = "admin@localhost.com",
            //         NormalizedUserName = "ADMIN@LOCALHOST.COM",
            //         PasswordHash = hasher.HashPassword(null, "P@ssword1"),
            //         EmailConfirmed = true

            //     },
            //     new AppUser
            //     {
            //         Id = "NormalUser",
            //         Email = "user@localhost.com",
            //         NormalizedEmail = "USER@LOCALHOST.COM",
            //         FirstName = "System",
            //         LastName = "User",
            //         UserName = "user@localhost.com",
            //         NormalizedUserName = "USER@LOCALHOST.COM",
            //         PasswordHash = hasher.HashPassword(null, "P@ssword1"),
            //         EmailConfirmed = true

            //     }
            //);
        }
    }
}
