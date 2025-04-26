using MediatR;

namespace CleanArchitectureSystem.Application.Features.AppUser.Commands.Update
{
    // If you carry out an operation where you don't expect a return type we can always use Unit
    public class UpdateAppUserCommand : IRequest<Unit>
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
