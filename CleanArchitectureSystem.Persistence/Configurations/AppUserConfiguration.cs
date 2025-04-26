using CleanArchitectureSystem.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitectureSystem.Persistence.Configurations
{
    public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.HasData(
                 new AppUser
                 {
                     Id = 1,
                     Username = "test",
                     PasswordHash = "12345",
                     SecurityStamp = "testtss",
                     ConcurrenceStamp = "tesdasdsada",
                     Email = "test.gmail.com",
                     FirstName = "Jake",
                     LastName = "Umali",
                     IsActive = true,
                 }
               );

            builder.Property(q => q.Id)
              .IsRequired()
              .HasMaxLength(100);
        }
    }
}
