using System.ComponentModel.DataAnnotations;

namespace CleanArchitectureSystem.Application.Models.Identity
{
    public class UpdateRequest
    {
        [Required]
        public required string Username { get; set; }
        [Required]
        [MinLength(6)]
        public required string Password { get; set; }
        [Required]
        [MinLength(6)]
        public required string ConfirmPassword { get; set; }
        public bool IsActive { get; set; }
        public string UserRole { get; set; } = string.Empty;
    }
}
