using MediatR;

namespace CleanArchitectureSystem.Application.Features.AppUser.Commands.Delete
{
    public class DeleteAppUserCommand : IRequest<Unit>
    {
        public int Id { get; set; }
    }
}
