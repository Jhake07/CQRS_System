using CleanArchitectureSystem.Domain.Common;

namespace CleanArchitectureSystem.Domain
{
    public class AppUser : BaseEntity
    {
        public required string Username { get; set; }
        public required string PasswordHash { get; set; }
        public required string SecurityStamp { get; set; }
        public required string ConcurrenceStamp { get; set; }
        public required string Email { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required bool IsActive { get; set; }
        public DateTime LastLogin { get; set; }
    }
}
