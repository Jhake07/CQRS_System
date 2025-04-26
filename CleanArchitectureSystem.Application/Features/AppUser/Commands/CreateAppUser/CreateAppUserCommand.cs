using MediatR;

namespace CleanArchitectureSystem.Application.Features.AppUser.Commands.Create
{
    public class CreateAppUserCommand : IRequest<int>
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public string PasswordHash { get; set; } = string.Empty;
        public string SecurityStamp { get; set; } = string.Empty;
        public string ConcurrenceStamp { get; set; } = string.Empty;

    }
}
